using CaseManagement.BPMN.DelegateConfiguration.Results;
using CaseManagement.BPMN.Persistence;
using CaseManagement.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.DelegateConfiguration.Queries.Handlers
{
    public class SearchDelegateConfigurationQueryHandler : IRequestHandler<SearchDelegateConfigurationQuery, SearchResult<DelegateConfigurationResult>>
    {
        private readonly IDelegateConfigurationRepository _delegateConfigurationRepository;

        public SearchDelegateConfigurationQueryHandler(IDelegateConfigurationRepository delegateConfigurationRepository)
        {
            _delegateConfigurationRepository = delegateConfigurationRepository;
        }

        public Task<SearchResult<DelegateConfigurationResult>> Handle(SearchDelegateConfigurationQuery request, CancellationToken cancellationToken)
        {
            return _delegateConfigurationRepository.Search(request, cancellationToken);
        }
    }
}
