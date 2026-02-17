using Microsoft.Data.SqlClient;
using System.Data;

namespace CarGalleryAPI.Data
{
    public static class DbCreator
    {
        private static SqlConnection connection;

        private static string dbExistSql = @"SELECT CASE WHEN DB_ID('CarGalleryDB') IS NULL THEN 0 ELSE 1 END";

        private static string _seedAdminUsername = "admin";
        private static string? _seedAdminEmail = "support@cargallery.com";
        private static string? _seedAdminPassword;

        public static void Initialize(IConfiguration configuration)
        {
            var server = configuration["DatabaseConnection:Server"];
            var useWindowsAuthentication = configuration.GetValue<bool>("DatabaseConnection:useWindowsAuthentication");

            _seedAdminUsername = configuration["SeedAdmin:Username"] ?? _seedAdminUsername;
            _seedAdminEmail = configuration["SeedAdmin:Email"] ?? _seedAdminEmail;
            _seedAdminPassword = configuration["SeedAdmin:Password"];

            if (useWindowsAuthentication)
                connection = new SqlConnection($"Server={server};Integrated Security=True;TrustServerCertificate=true;");
            else
            {
                var username = configuration["DatabaseConnection:Username"];
                var password = configuration["DatabaseConnection:Password"];
                connection = new SqlConnection($"Server={server};User Id={username};Password={password};TrustServerCertificate=true;");
            }
        }

        public static void ValidateSeedAdminConfig()
        {
            if (string.IsNullOrWhiteSpace(_seedAdminPassword))
                throw new InvalidOperationException("SeedAdmin:Password must be set when creating the database. Set it via env var SeedAdmin__Password (or SEED_ADMIN_PASSWORD in .env when using docker compose).");
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
            string sqlQuery = File.ReadAllText("Initial.sql");

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
            if (string.IsNullOrWhiteSpace(_seedAdminPassword))
                throw new InvalidOperationException("SeedAdmin:Password must be set when seeding the default admin.");

            var seedAdminPasswordHash = Hash.HashPassword(_seedAdminPassword);

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

            sqlCommand = new SqlCommand("INSERT INTO Users (id, role_id, username, email, password) VALUES (NEWID(), 1, @username, @email, @password);", connection);
            sqlCommand.Parameters.AddWithValue("@username", _seedAdminUsername);
            sqlCommand.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(_seedAdminEmail) ? DBNull.Value : _seedAdminEmail);
            sqlCommand.Parameters.AddWithValue("@password", seedAdminPasswordHash);
            sqlCommand.ExecuteNonQuery();
        }
    }
}
