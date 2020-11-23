using MediatR;

namespace CaseManagement.BPMN.ProcessFile.Commands
{
    public class UpdateProcessFilePayloadCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Payload { get; set; }
    }
}
