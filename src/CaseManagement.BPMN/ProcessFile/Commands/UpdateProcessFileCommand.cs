using MediatR;

namespace CaseManagement.BPMN.ProcessFile.Commands
{
    public class UpdateProcessFileCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
    }
}
