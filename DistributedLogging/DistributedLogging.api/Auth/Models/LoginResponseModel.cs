namespace DistributedLogging.Web.Api.Auth.Models
{
    public class LoginResponseModel
    {
        public bool IsSucceeded { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
