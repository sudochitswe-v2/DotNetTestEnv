using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CSDotNetTranning.ConsoleApp.AdoDotNetExamples
{
    public class AdoDotNetExample
    {
        #region Private Properties
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new ()
        {
            DataSource = "(local)\\SQLEXPRESS",
            InitialCatalog = "Db_DotNetTranning",
            UserID = "sa",
            Password = "admin",
        };

        #endregion
        public void Read()
        {
            var query = "SELECT * FROM tbl_Blog";
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            var cmd = new SqlCommand(query, sqlConnection);
            var adapter = new SqlDataAdapter(cmd);
            var dt_Blog = new DataTable();
            adapter.Fill(dt_Blog);
            sqlConnection.Close();
            foreach (DataRow row in dt_Blog.Rows)
            {
                Console.WriteLine(row["BlogID"]);
                Console.WriteLine(row["BlogTitle"]);
                Console.WriteLine(row["BlogAuthor"]);
                Console.WriteLine(row["BlogContent"]);
                Console.WriteLine("------------------");
            }
        }
        public void Edit(int id)
        {
            var query = "SELECT * FROM tbl_Blog WHERE BlogID = @BlogID";
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            var cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue(parameterName: "@BlogID", id);
            var adapter = new SqlDataAdapter(cmd);
            var dt_Blog = new DataTable();
            adapter.Fill(dt_Blog);
            sqlConnection.Close();
            if (dt_Blog.Rows.Count == 0)
            {
                Console.WriteLine("Record not found.");
                return;
            }
            foreach (DataRow row in dt_Blog.Rows)
            {
                Console.WriteLine(row["BlogID"]);
                Console.WriteLine(row["BlogTitle"]);
                Console.WriteLine(row["BlogAuthor"]);
                Console.WriteLine(row["BlogContent"]);
                Console.WriteLine("------------------");
            }
        }
        public void Create(string title, string author, string content)
        {
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            var sql = @"INSERT INTO [dbo].[tbl_Blog]
                         ([BlogTitle],[BlogAuthor],[BlogContent])
                         VALUES
                        (@BlogTitle,@BlogAuthor,@BlogContent)";
            var cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue(parameterName: "@BlogTitle", title);
            cmd.Parameters.AddWithValue(parameterName: "@BlogAuthor", author);
            cmd.Parameters.AddWithValue(parameterName: "@BlogContent", content);
            var result = cmd.ExecuteNonQuery();
            var message = result > 0 ? "Create Success" : "Create Fail";
            Console.WriteLine(message);
            sqlConnection.Close();
        }
        public void Update(int id, string title, string author, string content)
        {
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            var sql = @"UPDATE [dbo].[tbl_Blog]
                          SET [BlogTitle] = @BlogTitle
                             ,[BlogAuthor] = @BlogAuthor
                             ,[BlogContent] = @BlogContent
                        WHERE BlogID = @BlogID";
            var cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue(parameterName: "@BlogID", id);
            cmd.Parameters.AddWithValue(parameterName: "@BlogTitle", title);
            cmd.Parameters.AddWithValue(parameterName: "@BlogAuthor", author);
            cmd.Parameters.AddWithValue(parameterName: "@BlogContent", content);
            var result = cmd.ExecuteNonQuery();
            var message = result > 0 ? "Update Success" : "Update Fail";
            Console.WriteLine(message);
            sqlConnection.Close();
        }
        public void Delete(int id)
        {
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            var sql = "DELETE FROM [dbo].[tbl_Blog] WHERE BlogID = @BlogID";
            var cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue(parameterName: "@BlogID", id);
            var result = cmd.ExecuteNonQuery();
            var message = result > 0 ? "Delete Success" : "Delete Fail";
            Console.WriteLine(message);
            sqlConnection.Close();
        }
    }
}
