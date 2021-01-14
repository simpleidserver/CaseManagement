using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefPresentationParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
