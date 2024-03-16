using Microsoft.EntityFrameworkCore;
using MissingHistoricalRecords.WebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace MissingHistoricalRecords.WebApi
{

    public class AppDbContext : DbContext
    {
        private readonly string _sqlConnectionString;
        public AppDbContext()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = "(local)\\SQLEXPRESS",
                InitialCatalog = "Db_DotNetTranning",
                UserID = "sa",
                Password = "admin",
                TrustServerCertificate = true,
            };
            _sqlConnectionString = builder.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_sqlConnectionString);

        }
        public IDbConnection CreateConnection() => new SqlConnection(_sqlConnectionString);
        public SqlConnection CreateSqlConnection()=>new SqlConnection(_sqlConnectionString);
        public DbSet<BookModel> Books { get; set; }
    }

}
