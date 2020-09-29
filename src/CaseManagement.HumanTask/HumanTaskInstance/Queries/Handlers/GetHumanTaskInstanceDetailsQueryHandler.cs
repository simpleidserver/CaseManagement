using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetHumanTaskInstanceDetailsQueryHandler : IRequestHandler<GetHumanTaskInstanceDetailsQuery, TaskInstanceDetailsResult>
    {
        private readonly ILogger<GetHumanTaskInstanceDetailsQueryHandler> _logger;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;

        public GetHumanTaskInstanceDetailsQueryHandler(
            ILogger<GetHumanTaskInstanceDetailsQueryHandler> logger,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository)
        {
            _logger = logger;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
        }

        public async Task<TaskInstanceDetailsResult> Handle(GetHumanTaskInstanceDetailsQuery request, CancellationToken cancellationToken)
        {
            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            return TaskInstanceDetailsResult.ToDto(humanTaskInstance);
        }
    }
}
