using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefInfoCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}
