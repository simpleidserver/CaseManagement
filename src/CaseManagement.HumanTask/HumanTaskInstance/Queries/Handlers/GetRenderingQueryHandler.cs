using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetRenderingQueryHandler : IRequestHandler<GetRenderingQuery, JObject>
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

        public async Task<JObject> Handle(GetRenderingQuery request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task '{request.HumanTaskInstanceId}' instance doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            return string.IsNullOrWhiteSpace(result.Rendering) ? new JObject()
            {
                { "type", "container" },
                { "children", new JArray() }
            } : JObject.Parse(result.Rendering);
        }
    }
}
