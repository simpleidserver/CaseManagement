using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.BPMN.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Queries.Handlers
{
    public class GetProcessInstanceQueryHandler : IRequestHandler<GetProcessInstanceQuery, ProcessInstanceResult>
    {
        private readonly IProcessInstanceQueryRepository _caseInstanceQueryRepository;
        private readonly ILogger<GetProcessInstanceQueryHandler> _logger;

        public GetProcessInstanceQueryHandler(
            IProcessInstanceQueryRepository caseInstanceQueryRepository,
            ILogger<GetProcessInstanceQueryHandler> logger)
        {
            _caseInstanceQueryRepository = caseInstanceQueryRepository;
            _logger = logger;
        }

        public async Task<ProcessInstanceResult> Handle(GetProcessInstanceQuery request, CancellationToken cancellationToken)
        {
            var result = await _caseInstanceQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"Process instance '{request.Id}' doesn't exist");
                throw new UnknownFlowInstanceException(string.Format(Global.UnknownProcessInstance, request.Id));
            }

            return ProcessInstanceResult.ToDto(result);
        }
    }
}
