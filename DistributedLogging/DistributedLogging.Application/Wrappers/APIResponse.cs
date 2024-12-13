

namespace DistributedLogging.Application.Wrappers
{
    public class APIResponse<T>
    {
        public APIResponse()
        {
        }
        public APIResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public APIResponse(string errorMessage)
        {
            Succeeded = false;
            Errors = new List<string>() { errorMessage };
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
