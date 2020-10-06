using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetHumanTaskInstanceSubTasksQueryHandler : IRequestHandler<GetHumanTaskInstanceSubTasksQuery, SubTasksResults>
    {
        private readonly ILogger<GetHumanTaskInstanceSubTasksQueryHandler> _logger;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;

        public GetHumanTaskInstanceSubTasksQueryHandler(
            ILogger<GetHumanTaskInstanceSubTasksQueryHandler> logger,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository)
        {
            _logger = logger;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
        }

        public async Task<SubTasksResults> Handle(GetHumanTaskInstanceSubTasksQuery request, CancellationToken cancellationToken)
        {
            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            var result = await _humanTaskInstanceQueryRepository.GetSubTasks(humanTaskInstance.AggregateId, cancellationToken);
            ICollection<TaskInstanceDetailsResult> content = result.Select(_ => TaskInstanceDetailsResult.ToDto(_)).ToList();
            return new SubTasksResults 
            { 
                Content = content
            };
        }
    }
}
