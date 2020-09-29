using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetHumanTaskInstanceDetailsQuery : IRequest<TaskInstanceDetailsResult>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
