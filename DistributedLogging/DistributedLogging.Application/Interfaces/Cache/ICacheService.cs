
using DistributedLogging.common;

namespace DistributedLogging.Application.Interfaces.Cache
{
    public interface ICacheService
    {
        Task<T> GetOrAdd<T>(string key, Func<Task<T>> requestAction, RedisCacheOptionType redisCacheOptionType = RedisCacheOptionType.Default);
        Task RemoveCacheWithPattern(string key);
    }
}
