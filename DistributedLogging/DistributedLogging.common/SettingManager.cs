using DistributedLogging.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DistributedLogging.common
{
    public static class SettingsManager
    {
        public static readonly IConfiguration Configuration;
        private static readonly IServiceProvider ServiceProvider;

        static SettingsManager()
        {
            var services = new ServiceCollection();

            // Build the configuration
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .Build();

            // Configure services
            services.Configure<JWTSettings>(options =>
                    Configuration.GetSection("JWTSettings").Bind(options));
            services.Configure<RedisSettings>(options =>
                    Configuration.GetSection("RedisSettings").Bind(options)); 
            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();
        }

       
        public static JWTSettings JWTSettings
        {
            get
            {
                // Resolve and return the settings
                return ServiceProvider.GetRequiredService<IOptions<JWTSettings>>().Value;
            }
        }
        public static RedisSettings  RedisSettings
        {
            get
            {
                // Resolve and return the settings
                return ServiceProvider.GetRequiredService<IOptions<RedisSettings>>().Value;
            }
        }
        public static string DefaultConnection
        {
            get
            {
                // Resolve and return the settings
                return GetConnectionString("DefaultConnection");
            }
        }
       
        private static string GetConnectionString(string connectionName)
        {
            return Configuration.GetConnectionString(connectionName);
        }
    }
}

