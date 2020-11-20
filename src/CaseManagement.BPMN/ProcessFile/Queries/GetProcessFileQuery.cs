using CaseManagement.BPMN.ProcessFile.Results;
using MediatR;

namespace CaseManagement.BPMN.ProcessFile.Queries
{
    public class GetProcessFileQuery : IRequest<ProcessFileResult>
    {
        public string Id { get; set; }
    }
}
