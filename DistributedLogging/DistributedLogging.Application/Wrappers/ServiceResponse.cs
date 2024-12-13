
using DistributedLogging.common;


namespace DistributedLogging.Application.Wrappers
{
    public class ServiceResponse<T>
    {
        public ServiceResponse()
        {
        }
        public ServiceResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public ServiceResponse(string errorMessage)
        {
            Succeeded = false;
            Errors = new List<string>() { errorMessage };
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public ServiceResponseStatus ServiceResponseStatus { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
    public class SuccessServiceResponse<T> : ServiceResponse<T>
    {
        public SuccessServiceResponse(T data)
        {
            Succeeded = true;
            ServiceResponseStatus = ServiceResponseStatus.Success;
            Data = data;
        }
        public SuccessServiceResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            ServiceResponseStatus = ServiceResponseStatus.Success;
            Data = data;
        }
    }
    public class ValidationErrorServiceResponse<T> : ServiceResponse<T>
    {
        public ValidationErrorServiceResponse()
        {
            Succeeded = false;
            ServiceResponseStatus = ServiceResponseStatus.ValidationError;
        }
        public ValidationErrorServiceResponse(string errorMessage)
        {
            Succeeded = false;
            ServiceResponseStatus = ServiceResponseStatus.ValidationError;
            Errors = new List<string>() { errorMessage };
        }
    }
    public class SavingErrorServiceResponse<T> : ServiceResponse<T>
    {
        public SavingErrorServiceResponse(string errorMessage)
        {
            Succeeded = false;
            ServiceResponseStatus = ServiceResponseStatus.SavingError;
            Errors = new List<string>() { errorMessage };
        }
    }
    public class NotFoundServiceResponse<T> : ServiceResponse<T>
    {
        public NotFoundServiceResponse(string errorMessage)
        {
            Succeeded = false;
            ServiceResponseStatus = ServiceResponseStatus.NotFound;
            Errors = new List<string>() { errorMessage };
        }
    }
}
