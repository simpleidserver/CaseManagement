using CaseManagement.CMMN.CaseFile.Results;
using MediatR;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    public class AddCaseFileCommand : IRequest<CreateCaseFileResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
        public string Owner { get; set; }
    }
}
