using DistributedLogging.Application.Interfaces.Cache;
using DistributedLogging.common;
using DistributedLogging.Common.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;



namespace ValU_CMS.Infrastructure.Presistence.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _cache;
        private readonly RedisSettings _redisSettings;
        private readonly IDatabaseAsync _redisDatabase;

        public RedisCacheService(IConnectionMultiplexer cache)
        {
            _redisSettings = SettingsManager.RedisSettings;
            _cache = cache;
            _redisDatabase = _cache.GetDatabase(_redisSettings.DefaultDatabase);
        }
        public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> requestAction, RedisCacheOptionType redisCacheOptionType = RedisCacheOptionType.Default)
        {
            if (!_redisSettings.IsEnabled.HasValue || !_redisSettings.IsEnabled.Value)
            {
                return await requestAction();
            }

            var options = await GetRedisCacheOptions(redisCacheOptionType);

            var cachedValue = await _redisDatabase.StringGetAsync(key);

            if (cachedValue.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(cachedValue);
            }
            var data = await requestAction();

            await _redisDatabase.StringSetAsync(key, JsonConvert.SerializeObject(data), options.AbsoluteExpirationRelativeToNow);

            return data;
        }
        public async Task RemoveCacheWithPattern(string key)
        {
            foreach (var ep in _cache.GetEndPoints())
            {
                var server = _cache.GetServer(ep);
                if (string.IsNullOrEmpty(key))
                {
                    await server.FlushAllDatabasesAsync();
                }
                else
                {
                    var keys = server.Keys(database: _redisSettings.DefaultDatabase,
                        pattern: SettingsManager.RedisSettings.InstanceName.ToLower() + key.ToLower().Trim() + "*").ToArray();
                    await _redisDatabase.KeyDeleteAsync(keys);
                }
            }
        }

        private async Task<DistributedCacheEntryOptions> GetRedisCacheOptions(RedisCacheOptionType redisCacheOptionType)
        {
            return redisCacheOptionType switch
            {
                RedisCacheOptionType.Default => new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_redisSettings.EntryOptions.AbsoluteExpirationRelativeToNowInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_redisSettings.EntryOptions.SlidingExpirationInMiutes),
                },
                RedisCacheOptionType.Long => new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_redisSettings.EntryOptions.LongAbsoluteExpirationRelativeToNowInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_redisSettings.EntryOptions.SlidingExpirationInMiutes)
                }
            };
        }
    }
}
