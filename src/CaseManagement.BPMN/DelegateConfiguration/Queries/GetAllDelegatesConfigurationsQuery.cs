using MediatR;
using System.Collections.Generic;

namespace CaseManagement.BPMN.DelegateConfiguration.Queries
{
    public class GetAllDelegatesConfigurationsQuery : IRequest<IEnumerable<string>>
    {
    }
}
