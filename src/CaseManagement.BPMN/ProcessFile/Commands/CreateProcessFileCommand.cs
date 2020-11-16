using CaseManagement.BPMN.ProcessFile.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessFile.Commands
{
    public class CreateProcessFileCommand : IRequest<CreateProcessFileResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
    }
}
