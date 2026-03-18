using Microsoft.Extensions.Configuration;

namespace FinanceManagementSystem.Util
{
    public static class DBPropertyUtil
    {
        /// <summary>
        /// Reads the connection string from a configuration file.
        /// This is the C# equivalent of the DBPropertyUtil described in the assignment.
        /// </summary>
        /// <param name="configFileName">Configuration file name (e.g. appsettings.json).</param>
        /// <param name="connectionName">Named connection string key (e.g. FinanceContext).</param>
        public static string GetConnectionString(string configFileName, string connectionName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString(connectionName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{connectionName}' not found in '{configFileName}'.");
            }

            return connectionString;
        }
    }
}

