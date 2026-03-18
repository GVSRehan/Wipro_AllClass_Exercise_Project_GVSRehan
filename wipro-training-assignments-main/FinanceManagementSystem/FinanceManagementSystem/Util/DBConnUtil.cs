using System.Data.SqlClient;

namespace FinanceManagementSystem.Util
{
    public static class DBConnUtil
    {
        /// <summary>
        /// Returns an open SqlConnection using the connection string
        /// resolved by DBPropertyUtil from appsettings.json.
        /// This mirrors the DBConnUtil requirement from the assignment.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var connectionString = DBPropertyUtil.GetConnectionString("appsettings.json", "FinanceContext");

            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}

