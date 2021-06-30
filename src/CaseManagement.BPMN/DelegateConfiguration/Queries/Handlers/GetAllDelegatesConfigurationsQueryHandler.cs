using CaseManagement.BPMN.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.DelegateConfiguration.Queries.Handlers
{
    public class GetAllDelegatesConfigurationsQueryHandler : IRequestHandler<GetAllDelegatesConfigurationsQuery, IEnumerable<string>>
    {
        private readonly IDelegateConfigurationRepository _delegateConfigurationRepository;

        public GetAllDelegatesConfigurationsQueryHandler(IDelegateConfigurationRepository delegateConfigurationRepository)
        {
            _delegateConfigurationRepository = delegateConfigurationRepository;
        }

        public Task<IEnumerable<string>> Handle(GetAllDelegatesConfigurationsQuery request, CancellationToken cancellationToken)
        {
            return _delegateConfigurationRepository.GetAll(cancellationToken);
        }
    }
}
