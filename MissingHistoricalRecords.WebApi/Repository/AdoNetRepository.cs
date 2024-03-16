using Dapper;
using Microsoft.EntityFrameworkCore;
using MissingHistoricalRecords.WebApi.Models;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
    }
}
