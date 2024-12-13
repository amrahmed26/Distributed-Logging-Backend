using DistributedLogging.Application.Features.LoggedEntries.Commands.CreateLoggedEntry;
using DistributedLogging.Application.Features.LoggedEntries.Queries.GetLoggedEntriesList;
using DistributedLogging.Application.Features.LoggedEntries.Queries.GetLoggedEntryById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DistributedLogging.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoggedEntriesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetLoggedEntriesList([FromQuery] GetLoggedEntriesListQuery query) => GetAPIResponse(await Send(query));
        [HttpGet("GetLoggedEntriesById")]
        public async Task<IActionResult> GetLoggedEntriesById([FromQuery] int Id) => GetAPIResponse(await Send(new GetLoggedEntryByIdQuery(Id)));
        [HttpPost]
        public async Task<IActionResult> PostLoggedEntries([FromBody] CreateLoggedEntryCommand createLoggedEntryCommand)
        {
            Log.Information("Service: {Service}, Level: {Level}, Message: {Message}, Timestamp: {Timestamp}",
            createLoggedEntryCommand.Service, createLoggedEntryCommand.Level, createLoggedEntryCommand.Message, createLoggedEntryCommand.TimeStamp);
            return GetAPIResponse(await Send(createLoggedEntryCommand));
        }

    }
}
