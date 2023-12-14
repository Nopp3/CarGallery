using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace CarGalleryAPI.Data
{
    public static class DbCreator
    {
        private static SqlConnection connection;
        
        private static string dbExistSql = @"SELECT CASE WHEN DB_ID('CarGalleryDB') IS NULL THEN 0 ELSE 1 END";

        public static void Initialize(IConfiguration configuration)
        {
            var server = configuration["DatabaseConnection:Server"];
            var useWindowsAuthentication = configuration.GetValue<bool>("DatabaseConnection:useWindowsAuthentication");

            if (useWindowsAuthentication)
                connection = new SqlConnection($"Server={server};Integrated Security=True;TrustServerCertificate=true;");
            else
            {
                var username = configuration["DatabaseConnection:Username"];
                var password = configuration["DatabaseConnection:Password"];
                connection = new SqlConnection($"Server={server};User Id={username};Password={password};TrustServerCertificate=true;");
            }
        }
        public static bool DoesDbExist()
        {
            SqlCommand sqlCommand = new SqlCommand(dbExistSql, connection);

            connection.Open();

            int result = (int)sqlCommand.ExecuteScalar();

            connection.Close();

            if (result == 0)
                return false;
            else
                return true;
        }

        public static void CreateDatabase()
        {
            string sqlQuery = File.ReadAllText("initial.sql");

            SqlCommand sqlCommand = new SqlCommand("CREATE DATABASE CarGalleryDB;", connection);

            connection.Open();

            sqlCommand.ExecuteNonQuery();

            sqlCommand = new SqlCommand(sqlQuery, connection);
            sqlCommand.ExecuteNonQuery();

            AddDataFromExcel();

            connection.Close();
        }

        private static void AddDataFromExcel()
        {
            DataSet ds = ExcelReader.ImportFromExcel();
            SqlCommand sqlCommand;
            foreach (DataTable table in ds.Tables)
            {
                string sql = "";
                sql += $"INSERT INTO {table.TableName} (";
                int counter = 0;
                foreach (DataRow row in table.Rows)
                {
                    switch (counter){
                        case 0:
                            sql += $"{row["Column0"]}, {row["Column1"]}) VALUES ";
                            break;
                        case 1:
                            sql += $"('{row["Column0"]}', '{row["Column1"]}')";
                            break;
                        default:
                            sql += $",('{row["Column0"]}', '{row["Column1"]}')";
                            break;
                    }
                    counter++;
                }
                sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand = new SqlCommand("INSERT INTO Users (id, role_id, username, email, password) " +
                "VALUES (NEWID(), 1, 'admin', 'support@cargallery.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918');", connection);
            sqlCommand.ExecuteNonQuery();
        }
    }
}