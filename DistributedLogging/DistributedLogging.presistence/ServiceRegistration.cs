using DistributedLogging.Application.Interfaces.Cache;
using DistributedLogging.common;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ValU_CMS.Infrastructure.Presistence.Cache;
using DistributedLogging.Application.Interfaces.Repositories;
using DistributedLogging.Infrastructure.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using DistributedLogging.Presistence.Contexts;

namespace DistributedLogging.Presistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPresistenceInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(SettingsManager.DefaultConnection));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #region Cache
            var redisConfigSection = SettingsManager.RedisSettings;

            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { $"{redisConfigSection.Server}:{redisConfigSection.Port}" },
                AllowAdmin = true,
                AbortOnConnectFail = false,
            }));

            services.AddSingleton<ICacheService, RedisCacheService>();
            #endregion
            return services;
        }

    }
}
