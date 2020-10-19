using CaseManagement.HumanTask.HumanTaskDef.Results;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Queries
{
    public class GetHumanTaskDefQuery : IRequest<HumanTaskDefResult>
    {
        public string Id { get; set; }
    }
}
