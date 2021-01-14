using CaseManagement.HumanTask.Domains;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefPresentationElementCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public PresentationElementUsages Usage { get; set; }
        public string Language { get; set; }
    }
}
