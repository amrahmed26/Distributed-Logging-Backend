using DistributedLogging.Application.Interfaces.Repositories;
using DistributedLogging.Application.Wrappers;
using DistributedLogging.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLogging.Application.Features.LoggedEntries.Queries.GetLoggedEntryById
{
    public class GetLoggedEntryByIdQuery:IRequest<ServiceResponse<LoggedEntry>>
    {
        public int Id { get; set; }

        public GetLoggedEntryByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetLoggedEntryByIdQueryHandler:IRequestHandler<GetLoggedEntryByIdQuery, ServiceResponse<LoggedEntry>>
    {
        private readonly IGenericRepository<LoggedEntry> _repository;

        public GetLoggedEntryByIdQueryHandler(IGenericRepository<LoggedEntry> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<LoggedEntry>> Handle(GetLoggedEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var entity=await _repository.GetByIdAsync(request.Id);
            return new ServiceResponse<LoggedEntry>(entity);
        }
    }
}
