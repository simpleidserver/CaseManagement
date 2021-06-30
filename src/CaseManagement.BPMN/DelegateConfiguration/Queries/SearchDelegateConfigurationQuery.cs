using CaseManagement.BPMN.DelegateConfiguration.Results;
using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using MediatR;

namespace CaseManagement.BPMN.DelegateConfiguration.Queries
{
    public class SearchDelegateConfigurationQuery : BaseSearchParameter, IRequest<SearchResult<DelegateConfigurationResult>>
    {
    }
}
