using DistributedLogging.Application.Helpers;
using DistributedLogging.Application.Interfaces.Cache;
using DistributedLogging.Application.Interfaces.Repositories;
using DistributedLogging.Application.Wrappers;
using DistributedLogging.Domain.Entities;
using MediatR;
namespace DistributedLogging.Application.Features.LoggedEntries.Queries.GetLoggedEntriesList
{
    public class GetLoggedEntriesListQuery:IRequest<ServiceResponse<PagedResponse<List<LoggedEntry>>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 1000;
        public string? Service { get; set; }
        public string? Level { get; set; }
        public string? Message { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

    } 
    public class GetLoggedEntriesListQueryHandler:IRequestHandler<GetLoggedEntriesListQuery, ServiceResponse<PagedResponse<List<LoggedEntry>>>>
    {
        private readonly IGenericRepository<LoggedEntry> _repository;
        private readonly ICacheService _cacheService;

        public GetLoggedEntriesListQueryHandler(IGenericRepository<LoggedEntry> repository,ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<ServiceResponse<PagedResponse<List<LoggedEntry>>>> Handle(GetLoggedEntriesListQuery request, CancellationToken cancellationToken)
        {
            Func<Task<ServiceResponse<PagedResponse<List<LoggedEntry>>>>> actionMethod = async () =>
            {
                // Fetching data
                var loggedEntries = _repository.GetAll();
                var loggedEntriesQuery = LoggedEntryFilter.ApplyFilters(loggedEntries, request);


                var count =  loggedEntries.LongCount();
                var data = loggedEntriesQuery.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
                var pagedResponse = new PagedResponse<List<LoggedEntry>>(data, request.Page, request.PageSize, count);
                // Return data
                return new SuccessServiceResponse<PagedResponse<List<LoggedEntry>>>(pagedResponse);
            };
            string cacheKey = CacheHelper.BuildCachKey("loggedEntries", request);
            return await _cacheService.GetOrAdd(cacheKey, actionMethod);
        }
    }
    public static class LoggedEntryFilter
    {
        public static IQueryable<LoggedEntry> ApplyFilters(IQueryable<LoggedEntry> query, GetLoggedEntriesListQuery filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.Service))
            {
                query = query.Where(entry => entry.Service == filterModel.Service);
            }

            if (!string.IsNullOrEmpty(filterModel.Level))
            {
                query = query.Where(entry => entry.Level == filterModel.Level);
            }

            if (!string.IsNullOrEmpty(filterModel.Message))
            {
                query = query.Where(entry => entry.Message.Contains(filterModel.Message));
            }

            if (filterModel.From.HasValue)
            {
                query = query.Where(entry => entry.TimeStamp >= filterModel.From.Value);
            }

            if (filterModel.To.HasValue)
            {
                query = query.Where(entry => entry.TimeStamp <= filterModel.To.Value);
            }

            return query;
        }
    }

}


