using MediatR;
using System.Collections.Generic;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefPresentationParametersCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public ICollection<PresentationElementDefinitionResult> PresentationElements { get; set; }
        public ICollection<PresentationParameterResult> PresentationParameters { get; set; }
    }
}
