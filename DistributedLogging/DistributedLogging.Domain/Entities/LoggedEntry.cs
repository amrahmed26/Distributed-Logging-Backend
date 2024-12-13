

namespace DistributedLogging.Domain.Entities
{
    public class LoggedEntry
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
