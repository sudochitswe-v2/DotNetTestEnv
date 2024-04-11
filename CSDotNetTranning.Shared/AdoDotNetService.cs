using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace CSDotNetTranning.Shared
{
    public class AdoDotNetService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;
        public AdoDotNetService(string connectionString)
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }
        public T QueryFirstOrDefault<T>(string query, List<SqlParameter>? parameters = null)
        {
            var connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            var cmd = new SqlCommand(query, connection);
            if (parameters is not null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            string json = JsonConvert.SerializeObject(dt);
            var list = JsonConvert.DeserializeObject<List<T>>(json);
            return list![0];
        }
        public List<T>? Query<T>(string query, List<SqlParameter>? parameters = null)
        {
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            sqlConnection.Open();
            var cmd = new SqlCommand(query, sqlConnection);
            if (parameters is not null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            sqlConnection.Close();
            string json = JsonConvert.SerializeObject(dt);
            var list = JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }
        public int Execute(string sql, List<SqlParameter>? parameters = null)
        {
            var sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            var cmd = new SqlCommand(sql, sqlConnection);
            if (parameters is not null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            var result = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return result;
        }
    }
}
