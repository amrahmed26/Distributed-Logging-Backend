

namespace DistributedLogging.common
{
    public enum RedisCacheOptionType
    {
        Default,
        Long
    }
    public enum ServiceResponseStatus
    {
        Success,
        ValidationError,
        SavingError,
        UnKnownError,
        NotFound
    }
}
