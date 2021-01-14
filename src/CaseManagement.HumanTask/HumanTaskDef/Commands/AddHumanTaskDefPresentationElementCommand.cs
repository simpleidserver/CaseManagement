using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefPresentationElementCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public PresentationElementDefinitionResult PresentationElement { get; set; }
    }
}