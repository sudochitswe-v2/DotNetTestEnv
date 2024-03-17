using Dapper;
using Microsoft.EntityFrameworkCore;
using MissingHistoricalRecords.WebApi.Models;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Text;

namespace MissingHistoricalRecords.WebApi.Repository
{
    public class AdoNetRepository
    {
        private readonly AppDbContext _context;
        public AdoNetRepository(AppDbContext context)
        {
            _context = context;
        }
        private static IEnumerable<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
        {
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var obj = new T();
                foreach (DataColumn col in dt.Columns)
                {
                    var prop = obj.GetType().GetProperty(col.ColumnName);
                    if (prop != null && row[col] != DBNull.Value)
                        prop.SetValue(obj, row[col]);
                }
                list.Add(obj);
            }
            return list.ToArray();
        }
        private static T ConvertDataRowToObject<T>(DataRow row) where T : new()
        {
            var obj = new T();
            foreach (DataColumn col in row.Table.Columns)
            {
                var prop = obj.GetType().GetProperty(col.ColumnName);
                if (prop != null)
                {
                    object value = row[col];
                    if (value != DBNull.Value)
                    {
                        // Try direct assignment or use a type converter if necessary
                        try
                        {
                            prop.SetValue(obj, value);
                        }
                        catch (InvalidCastException)
                        {
                            // Attempt conversion using a type converter if the property type is different
                            var converter = TypeDescriptor.GetConverter(prop.PropertyType);
                            if (converter.CanConvertFrom(value.GetType()))
                            {
                                value = converter.ConvertFrom(value);
                                prop.SetValue(obj, value);
                            }
                        }
                    }
                }
            }
            return obj;
        }
        public IEnumerable<BookModel> GetBooks()
        {
            var sql = "SELECT * FROM tbl_Book";
            using var connection = _context.CreateSqlConnection();
            connection.Open();
            var sqlCmd = new SqlCommand(sql, connection);
            var adapter = new SqlDataAdapter(sqlCmd);
            var dt_Book = new DataTable();
            adapter.Fill(dt_Book);
            connection.Close();
            var list = ConvertDataTableToList<BookModel>(dt_Book);
            return list;
        }
        public BookModel? GetBook(int id)
        {
            var bookIdName = nameof(BookModel.BookId);
            var sql = $"SELECT TOP 1 * FROM tbl_Book WHERE {bookIdName}=@{bookIdName}";
            using var connection = _context.CreateSqlConnection();
            connection.Open();
            var sqlCmd = new SqlCommand(sql, connection);
            sqlCmd.Parameters.AddWithValue(parameterName: $"@{bookIdName}", id);
            var adapter = new SqlDataAdapter(sqlCmd);
            var dt_Book = new DataTable();
            adapter.Fill(dt_Book);
            connection.Close();
            if (dt_Book.Rows.Count == 0) return null;
            var row = dt_Book.Rows[0];
            var book = ConvertDataRowToObject<BookModel>(row);
            return book;
        }
        public int CreateBook(BookModel createModel)
        {
            using var connection = _context.CreateSqlConnection();
            var properties = createModel
                .GetType()
                .GetProperties()
                .Where(prop => prop.Name != nameof(createModel.BookId));
            string columns = string.Join(", ", properties.Select(p => p.Name));
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            string sql = @$"INSERT INTO tbl_Book ({columns}) 
                             VALUES ({values})";
            connection.Open();
            using var trx = connection.BeginTransaction();
            var sqlCmd = new SqlCommand(sql, connection, trx);
            foreach (var property in properties)
            {
                sqlCmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(createModel));
            }
            var result = sqlCmd.ExecuteNonQuery();
            return result;
        }
        public int UpdateBook(int id, BookModel editModel)
        {
            using var connection = _context.CreateSqlConnection();
            var bookIdName = nameof(editModel.BookId);
            var properties = editModel
                .GetType()
                .GetProperties()
                .Where(prop => prop.Name != nameof(editModel.BookId));
            string values = string.Join(", ", properties.Select(p => $"{p.Name}=@{p.Name}"));
            string sql = @$"UPDATE tbl_Book SET {values} WHERE {bookIdName}=@{bookIdName}";
            connection.Open();
            using var trx = connection.BeginTransaction();
            var sqlCmd = new SqlCommand(sql, connection, trx);
            sqlCmd.Parameters.AddWithValue($"@{bookIdName}", id);
            foreach (var property in properties)
            {
                sqlCmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(editModel));
            }
            var result = sqlCmd.ExecuteNonQuery();
            return result;
        }
        public int DeleteBook(BookModel deleteModel)
        {
            using var connection = _context.CreateSqlConnection();
            var bookIdName = nameof(deleteModel.BookId);
            var sql = $"DELETE FROM tbl_Book WHERE {bookIdName}=@{bookIdName}";
            connection.Open();
            using var trx = connection.BeginTransaction();
            var sqlCmd = new SqlCommand(sql, connection, trx);
            sqlCmd.Parameters.AddWithValue($"@{bookIdName}", deleteModel.BookId);
            var result = sqlCmd.ExecuteNonQuery();
            return result;
        }
        public IEnumerable<ContentModel> GetBookContents(int bookId, int? pageNo)
        {
            var bookIdName = nameof(ContentModel.BookId);
            var pageNoName = nameof(ContentModel.PageNo);
            var parameters = new DynamicParameters();
            parameters.Add($"@{bookIdName}", bookId);
            var sql = new StringBuilder();
            sql.Append($"SELECT * FROM tbl_Content WHERE {bookIdName}=@{bookIdName}");
            if (pageNo.HasValue)
            {
                parameters.Add($"@{pageNoName}", pageNo);
                sql.Append($" AND {pageNoName}=@{pageNoName}");
            }
            using var connection = _context.CreateSqlConnection();
            connection.Open();
            var sqlCmd = new SqlCommand(sql.ToString(), connection);
            if (pageNo.HasValue)
            {
                sqlCmd.Parameters.AddWithValue(parameterName: $"@{pageNoName}", pageNo);
            }
            sqlCmd.Parameters.AddWithValue(parameterName: $"@{bookIdName}", bookId);

            var adapter = new SqlDataAdapter(sqlCmd);
            var dt_Content = new DataTable();
            adapter.Fill(dt_Content);
            connection.Close();
            var list = ConvertDataTableToList<ContentModel>(dt_Content);
            return list;
        }
        public ContentModel? GetCotent(int contentId)
        {
            var contentIdName = nameof(ContentModel.ContentId);
            var sql = $"SELECT TOP 1 * FROM tbl_Content WHERE {contentIdName}=@{contentIdName}";
            using var connection = _context.CreateSqlConnection();
            connection.Open();
            var sqlCmd = new SqlCommand(sql, connection);
            sqlCmd.Parameters.AddWithValue(parameterName: $"@{contentIdName}", contentId);
            var adapter = new SqlDataAdapter(sqlCmd);
            var dt_Cotent = new DataTable();
            adapter.Fill(dt_Cotent);
            connection.Close();
            if (dt_Cotent.Rows.Count == 0) return null;
            var row = dt_Cotent.Rows[0];
            var content = ConvertDataRowToObject<ContentModel>(row);
            return content;
        }
        public int CreateContent(ContentModel createModel)
        {
            using var connection = _context.CreateSqlConnection();
            var properties = createModel
                .GetType()
                .GetProperties()
                .Where(prop => prop.Name != nameof(createModel.ContentId));
            string columns = string.Join(", ", properties.Select(p => p.Name));
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            string sql = @$"INSERT INTO tbl_Content ({columns}) 
                             VALUES ({values})";
            connection.Open();
            using var trx = connection.BeginTransaction();
            var sqlCmd = new SqlCommand(sql, connection, trx);
            foreach (var property in properties)
            {
                sqlCmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(createModel));
            }
            var result = sqlCmd.ExecuteNonQuery();
            return result;
        }
        public int UpdateContent(int contentId, ContentModel editModel)
        {
            using var connection = _context.CreateSqlConnection();
            var contentIdName = nameof(editModel.ContentId);
            var properties = editModel
                .GetType()
                .GetProperties()
                .Where(prop => prop.Name != nameof(editModel.ContentId));
            string values = string.Join(", ", properties.Select(p => $"{p.Name}=@{p.Name}"));
            string sql = @$"UPDATE tbl_Content SET {values} WHERE {contentIdName}=@{contentIdName}";
            connection.Open();
            using var trx = connection.BeginTransaction();
            var sqlCmd = new SqlCommand(sql, connection, trx);
            sqlCmd.Parameters.AddWithValue($"@{contentIdName}", contentId);
            foreach (var property in properties)
            {
                sqlCmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(editModel));
            }
            var result = sqlCmd.ExecuteNonQuery();
            return result;
        }
        public int DeleteContent(ContentModel deleteModel)
        {
            using var connection = _context.CreateSqlConnection();
            var contentIdName = nameof(deleteModel.ContentId);
            var sql = $"DELETE FROM tbl_Content WHERE {contentIdName}=@{contentIdName}";
            connection.Open();
            using var trx = connection.BeginTransaction();
            var sqlCmd = new SqlCommand(sql, connection, trx);
            sqlCmd.Parameters.AddWithValue($"@{contentIdName}", deleteModel.ContentId);
            var result = sqlCmd.ExecuteNonQuery();
            return result;
        }
    }
}
