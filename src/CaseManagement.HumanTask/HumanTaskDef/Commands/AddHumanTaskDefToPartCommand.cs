using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefToPartCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadlineId { get; set; }
        public string EscalationId { get; set; }
        public ToPartResult ToPart { get; set; }
    }
}
