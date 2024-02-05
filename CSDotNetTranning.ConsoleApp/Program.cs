// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Data.SqlClient;

Console.WriteLine("Hello, World!");

var sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
{
    DataSource = "(local)\\SQLEXPRESS",
    InitialCatalog = "Db_DotNetTranning",
    UserID = "sa",
    Password = "admin",
};
//var constring = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=eLoadPortal;Connect Timeout=120;Min Pool Size=5;Integrated Security=False;user id=sa;password=admin";
//Console.WriteLine(constring);
var query = "SELECT * FROM tbl_Blog";
Console.WriteLine(sqlConnectionStringBuilder.ConnectionString);
var sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
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
    Console.WriteLine(row["BlogAurthor"]);
    Console.WriteLine(row["BlogContent"]);
    Console.WriteLine("------------------");
}
Console.ReadKey();