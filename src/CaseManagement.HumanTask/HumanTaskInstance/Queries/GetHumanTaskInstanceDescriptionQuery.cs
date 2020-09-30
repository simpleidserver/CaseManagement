using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetHumanTaskInstanceDescriptionQuery : IRequest<TaskDescriptionResult>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
