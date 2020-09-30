using CaseManagement.Common.Results;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetHumanTaskInstanceHistoryQueryHandler : IRequestHandler<GetHumanTaskInstanceHistoryQuery, SearchResult<TaskInstanceHistoryResult>>
    {
        private readonly ILogger<GetHumanTaskInstanceHistoryQueryHandler> _logger;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;

        public GetHumanTaskInstanceHistoryQueryHandler(
            ILogger<GetHumanTaskInstanceHistoryQueryHandler> logger,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository)
        {
            _logger = logger;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
        }

        public async Task<SearchResult<TaskInstanceHistoryResult>> Handle(GetHumanTaskInstanceHistoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskInstanceQueryRepository.FindHumanTaskInstanceHistory(new FindHumanTaskInstanceHistoryParameter
            {
                HumanTaskInstanceId = request.HumanTaskInstanceId
            }, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            return new SearchResult<TaskInstanceHistoryResult>
            {
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength,
                Content = result.Content.Select(_ => TaskInstanceHistoryResult.ToDto(_, request.IncludeData))
            };
        }
    }
}
