using CaseManagement.BPMN.ProcessFile.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessFile.Commands
{
    public class PublishProcessFileCommand : IRequest<PublishProcessFileResult>
    {
        public string Id { get; set; }
    }
}
