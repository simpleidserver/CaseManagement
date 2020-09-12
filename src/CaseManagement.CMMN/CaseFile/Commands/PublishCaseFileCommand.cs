using MediatR;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    public class PublishCaseFileCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}
