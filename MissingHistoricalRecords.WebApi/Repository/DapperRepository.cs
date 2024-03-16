using MissingHistoricalRecords.WebApi.Models;
using System.Data.SqlClient;
using Dapper;

namespace MissingHistoricalRecords.WebApi.Repository
{
    public class DapperRepository
    {
        private readonly AppDbContext _dbContext;
        public DapperRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public IEnumerable<BookModel> GetBooks()
        {
            var sql = "SELECT * FROM tbl_Book";
            using var connection = _dbContext.CreateConnection();
            return connection.Query<BookModel>(sql);
        }
        public BookModel? GetBook(int id)
        {
            var bookIdName = $"{nameof(BookModel.BookId)}";
            var parameter = new DynamicParameters();
            parameter.Add($"@{bookIdName}", id);
            var sql = $"SELECT * FROM tbl_Book WHERE {bookIdName} = @{bookIdName}";
            using var connection = _dbContext.CreateConnection();
            return connection.QueryFirstOrDefault<BookModel?>(sql, parameter);
        }
        public int CreateBook(BookModel createModel)
        {
            var properties = createModel
                .GetType()
                .GetProperties()
                .Where(prop => prop.Name != nameof(createModel.BookId));
            var parameter = new DynamicParameters();
            foreach (var property in properties)
            {
                parameter.Add($"@{property.Name}", property.GetValue(createModel));
            }
            string columns = string.Join(", ", properties.Select(p => p.Name));
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            string sql = @$"INSERT INTO tbl_Book ({columns}) 
                             VALUES ({values})";
            using var connection = _dbContext.CreateConnection();
            connection.Open();
            using var trx = connection.BeginTransaction();
            var res = connection.Execute(sql, parameter, trx);
            return res;
        }
        public int UpdateBook(int id, BookModel editModel)
        {
            var bookIdName = nameof(editModel.BookId);
            var properties = editModel
                .GetType()
                .GetProperties()
                .Where(prop => prop.Name != nameof(editModel.BookId));
            var parameter = new DynamicParameters();
            parameter.Add($"@{bookIdName}", id);
            foreach (var property in properties)
            {
                parameter.Add($"@{property.Name}", property.GetValue(editModel));
            }
            string values = string.Join(", ", properties.Select(p => $"{p.Name}=@{p.Name}"));
            string sql = @$"UPDATE tbl_Book SET {values} WHERE {bookIdName}=@{bookIdName}";
            using var connection = _dbContext.CreateConnection();
            connection.Open();
            using var trx = connection.BeginTransaction();
            var res = connection.Execute(sql, parameter, trx);
            return res;
        }
        public int DeleteBook(BookModel deleteModel)
        {
            var bookIdName = nameof(deleteModel.BookId);
            var parameter = new DynamicParameters();
            parameter.Add($"@{bookIdName}", deleteModel.BookId);
            var sql = $"DELETE FROM tbl_Book WHERE {bookIdName}=@{bookIdName}";
            using var connection = _dbContext.CreateConnection();
            connection.Open();
            using var trx = connection.BeginTransaction();
            var res = connection.Execute(sql, parameter, trx);
            return res;
        }
    }
}
