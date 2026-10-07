using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    internal class DatabaseManager
    {
        private static readonly string connectionString =
            "Server=localhost;Database=CareerPathDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}