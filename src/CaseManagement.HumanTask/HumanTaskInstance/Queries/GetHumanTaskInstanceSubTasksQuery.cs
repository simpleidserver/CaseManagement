using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetHumanTaskInstanceSubTasksQuery : IRequest<SubTasksResults>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
