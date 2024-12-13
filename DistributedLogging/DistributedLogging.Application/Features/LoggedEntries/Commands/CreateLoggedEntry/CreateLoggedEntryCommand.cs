using DistributedLogging.Application.Interfaces.Cache;
using DistributedLogging.Application.Interfaces.Repositories;
using DistributedLogging.Application.Wrappers;
using DistributedLogging.Domain.Entities;
using MediatR;

namespace DistributedLogging.Application.Features.LoggedEntries.Commands.CreateLoggedEntry
{
    public class CreateLoggedEntryCommand:IRequest<ServiceResponse<CreateLoggedEntryCommand>>
    {
        public string Service { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class CreateLoggedEntryCommandHandler : IRequestHandler<CreateLoggedEntryCommand, ServiceResponse<CreateLoggedEntryCommand>>
    {
        private readonly IGenericRepository<LoggedEntry> _repository;
        private readonly ICacheService _cacheService;

        public CreateLoggedEntryCommandHandler(IGenericRepository<LoggedEntry> repository,ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<ServiceResponse<CreateLoggedEntryCommand>> Handle(CreateLoggedEntryCommand request, CancellationToken cancellationToken)
        {
            var Entity=new LoggedEntry { Service = request.Service,Level=request.Service,Message=request.Message,TimeStamp=request.TimeStamp};
           await _repository.AddAsync(Entity);
            var result = await _repository.SaveChangesAsync();
            if (result)
            {
              await  _cacheService.RemoveCacheWithPattern("loggedEntries");
                return new SuccessServiceResponse<CreateLoggedEntryCommand>(request);
            }
            return new SavingErrorServiceResponse<CreateLoggedEntryCommand>("Error While Saving Entity");
        }
    }
}
