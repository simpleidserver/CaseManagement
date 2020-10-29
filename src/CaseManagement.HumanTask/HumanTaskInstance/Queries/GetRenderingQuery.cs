using CaseManagement.HumanTask.Domains;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetRenderingQuery : IRequest<Rendering>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
