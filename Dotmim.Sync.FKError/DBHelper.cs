using Microsoft.Extensions.Configuration;


namespace Dotmim.Sync.FKError
{
    public class DBHelper
    {
        private static IConfiguration configuration;

        static DBHelper()
        {
            configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", false, true)
              .AddJsonFile("appsettings.local.json", true, true)
              .Build();

        }

        public static string GetConnectionString(string connectionStringName) =>
            configuration.GetSection("ConnectionStrings")[connectionStringName];

        public static string GetDatabaseConnectionString(string dbName) =>
            string.Format(configuration.GetSection("ConnectionStrings")["SqlConnection"], dbName);
    }
}