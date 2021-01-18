using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string ParameterId { get; set; }
    }
}
