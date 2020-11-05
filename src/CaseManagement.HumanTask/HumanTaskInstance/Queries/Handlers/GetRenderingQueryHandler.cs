using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetRenderingQueryHandler : IRequestHandler<GetRenderingQuery, ICollection<RenderingElement>>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly ILogger<GetRenderingQueryHandler> _logger;

        public GetRenderingQueryHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            ILogger<GetRenderingQueryHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _logger = logger;
        }

        public async Task<ICollection<RenderingElement>> Handle(GetRenderingQuery request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task '{request.HumanTaskInstanceId}' instance doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            return result.RenderingElements;
        }
    }
}
