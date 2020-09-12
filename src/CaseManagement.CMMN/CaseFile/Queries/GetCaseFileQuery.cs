using MediatR;
using CaseManagement.CMMN.CaseFile.Results;

namespace CaseManagement.CMMN.CaseFile.Queries
{
    public class GetCaseFileQuery : IRequest<CaseFileResult>
    {
        public string Id { get; set; }
    }
}
