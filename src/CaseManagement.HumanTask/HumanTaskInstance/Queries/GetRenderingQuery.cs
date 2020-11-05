using CaseManagement.HumanTask.Domains;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetRenderingQuery : IRequest<ICollection<RenderingElement>>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
