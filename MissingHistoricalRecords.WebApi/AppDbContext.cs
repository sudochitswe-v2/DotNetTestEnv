using Microsoft.EntityFrameworkCore;
using MissingHistoricalRecords.WebApi.Models;
using System.Data.SqlClient;

namespace MissingHistoricalRecords.WebApi
{

    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "(local)\\SQLEXPRESS",
                InitialCatalog = "Db_DotNetTranning",
                UserID = "sa",
                Password = "admin",
                TrustServerCertificate = true,
            };
            optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);
        }
        public DbSet<BookModel> Books { get; set; }
    }

}
