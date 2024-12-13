using DistributedLogging.Application.Wrappers;
using DistributedLogging.common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DistributedLogging.api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected IActionResult GetAPIResponse<T>(ServiceResponse<T> result) where T : class
        {
            var response = GetResponse(result);
            return result.ServiceResponseStatus switch
            {
                ServiceResponseStatus.Success => Ok(response),
                ServiceResponseStatus.ValidationError => BadRequest(response),
                ServiceResponseStatus.SavingError => BadRequest(response),
                ServiceResponseStatus.UnKnownError => StatusCode((int)HttpStatusCode.InternalServerError),
                ServiceResponseStatus.NotFound => NotFound(response),
                _ => BadRequest(response)
            };
        }
        protected async Task<T> Send<T>(IRequest<T> r)
        {
            return await Mediator.Send(r);
        }
        APIResponse<T> GetResponse<T>(ServiceResponse<T> result) where T : class
        {
            return new APIResponse<T>
            {
                Data = result.Data,
                Message = result.Message,
                Errors = result.Errors,
                Succeeded = result.Succeeded,
            };
        }
    }
}
