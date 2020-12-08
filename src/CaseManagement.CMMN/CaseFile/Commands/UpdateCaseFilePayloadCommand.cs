using MediatR;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    public class UpdateCaseFilePayloadCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Payload { get; set; }
    }
}
