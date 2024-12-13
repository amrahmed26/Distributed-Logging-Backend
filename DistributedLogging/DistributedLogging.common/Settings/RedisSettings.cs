using System;

namespace DistributedLogging.Common.Settings
{
    public class RedisSettings
    {
        public const string SectionName = "RedisSettings";
        public string? Server { get; set; }
        public string? Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? InstanceName { get; set; }
        public int DefaultDatabase { get; set; }
        public bool? IsEnabled { get; set; }
        public EntryOptions EntryOptions { get; set; }
    }
    public class EntryOptions
    {
        public double AbsoluteExpirationRelativeToNowInHours { get; set; } = 4;
        public double LongAbsoluteExpirationRelativeToNowInHours { get; set; } = 6;
        public double SlidingExpirationInMiutes { get; set; } = 10;
    }
}
