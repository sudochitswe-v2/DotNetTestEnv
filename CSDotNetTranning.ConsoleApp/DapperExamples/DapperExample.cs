using CSDotNetTranning.ConsoleApp.Models;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CSDotNetTranning.ConsoleApp.DapperExamples
{
    public class DapperExample
    {
        #region Private Properties
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new()
        {
            DataSource = "(local)\\SQLEXPRESS",
            InitialCatalog = "Db_DotNetTranning",
            UserID = "sa",
            Password = "admin",
        }; 
        #endregion
        public void Read()
        {
            var query = "SELECT * FROM tbl_Blog WHERE BlogID = @BlogID";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            IEnumerable<BlogModel> blogs = db.Query<BlogModel>(query);
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.BlogID);
                Console.WriteLine(blog.BlogTitle);
                Console.WriteLine(blog.BlogContent);
                Console.WriteLine(blog.BlogAuthor);
            }
        }
        public void Edit(int id)
        {
            var query = "SELECT * FROM tbl_Blog WHERE BlogID = @BlogID";
            var updateModel = new BlogModel() { BlogID = id };
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            var blog = db.Query<BlogModel>(query, updateModel).FirstOrDefault();
            if (blog is null)
            {
                Console.WriteLine("No Data Found");
                return;
            }
            Console.WriteLine(blog.BlogID);
            Console.WriteLine(blog.BlogTitle);
            Console.WriteLine(blog.BlogContent);
            Console.WriteLine(blog.BlogAuthor);
        }
        public void Create(string title, string author, string content)
        {
            var sql = @"INSERT INTO [dbo].[tbl_Blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
                VALUES
           (@BlogTitle,@BlogAuthor,@BlogContent)";
            var blog = new BlogModel
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content
            };
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(sql, blog);
            var message = result > 0 ? "Save Success" : "Save Fail";
            Console.WriteLine(message);
        }
        public void Update(int id, string title, string author, string content)
        {
            var sql = @"UPDATE [dbo].[tbl_Blog]
                          SET [BlogTitle] = @BlogTitle
                             ,[BlogAurthor] = @BlogAurthor
                             ,[BlogContent] = @BlogContent
                        WHERE BlogID = @BlogID";
            var blog = new BlogModel
            {
                BlogID = id,
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content
            };
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(sql, blog);
            var message = result > 0 ? "Update Success" : "Update Fail";
            Console.WriteLine(message);
        }
        public void Delete(int id)
        {
            var sql = @"DELETE FROM [dbo].[tbl_Blog]
                         WHERE BlogID = @BlogID";
            var blog = new BlogModel { BlogID = id };
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(sql,blog);
            var message = result > 0 ? "Delete Success" : "Delete Fail";
            Console.WriteLine(message);
        }
    }
}
