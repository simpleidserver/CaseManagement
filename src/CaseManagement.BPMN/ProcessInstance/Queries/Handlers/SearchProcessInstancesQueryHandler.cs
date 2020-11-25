using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.Common.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Queries.Handlers
{
    public class SearchProcessInstancesQueryHandler : IRequestHandler<SearchProcessInstancesQuery, SearchResult<ProcessInstanceResult>>
    {
        private readonly IProcessInstanceQueryRepository _processInstanceQueryRepository;

        public SearchProcessInstancesQueryHandler(IProcessInstanceQueryRepository processInstanceQueryRepository)
        {
            _processInstanceQueryRepository = processInstanceQueryRepository;
        }

        public async Task<SearchResult<ProcessInstanceResult>> Handle(SearchProcessInstancesQuery request, CancellationToken cancellationToken)
        {
            var result = await _processInstanceQueryRepository.Find(new FindProcessInstancesParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                ProcessFileId = request.ProcessFileId,
                StartIndex = request.StartIndex,
                Status = (int?)request.Status
            }, cancellationToken);
            return new SearchResult<ProcessInstanceResult>
            {
                Content = result.Content.Select(_ => ProcessInstanceResult.ToDto(_)),
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength
            };
        }
    }
}
