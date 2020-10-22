using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefInputParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string ParameterName { get; set; }
    }
}
