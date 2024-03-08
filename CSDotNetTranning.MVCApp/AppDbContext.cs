using CSDotNetTranning.MVCApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace CSDotNetTranning.MVCApp
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
        public DbSet<BlogModel> Blogs { get; set; }
    }
}
