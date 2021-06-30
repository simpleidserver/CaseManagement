using CaseManagement.BPMN.DelegateConfiguration.Results;
using MediatR;

namespace CaseManagement.BPMN.DelegateConfiguration.Queries
{
    public class GetDelegateConfigurationQuery : IRequest<DelegateConfigurationResult>
    {
        public string Id { get; set; }
    }
}
