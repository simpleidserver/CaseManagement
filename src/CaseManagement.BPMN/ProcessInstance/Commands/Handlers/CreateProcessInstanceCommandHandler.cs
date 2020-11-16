using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.BPMN.Resources;
using CaseManagement.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class CreateProcessInstanceCommandHandler : IRequestHandler<CreateProcessInstanceCommand, ProcessInstanceResult>
    {
        private readonly ILogger<CreateProcessInstanceCommandHandler> _logger;
        private readonly IProcessFileQueryRepository _processFileQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CreateProcessInstanceCommandHandler(
            ILogger<CreateProcessInstanceCommandHandler> logger,
            IProcessFileQueryRepository processFileQueryRepository,
            ICommitAggregateHelper commitAggregateHelper)
        {
            _logger = logger;
            _processFileQueryRepository = processFileQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<ProcessInstanceResult> Handle(CreateProcessInstanceCommand request, CancellationToken cancellationToken)
        {
            var workflowDefinition = await _processFileQueryRepository.Get(request.ProcessFileId, cancellationToken);
            if (workflowDefinition == null)
            {
                _logger.LogError($"the process file '{request.ProcessFileId}' doesn't exist");
                throw new UnknownProcessFileException(string.Format(Global.UnknownProcessFile, request.ProcessFileId));
            }

            ProcessInstanceAggregate.New(workflowDefinition.AggregateId, workflowDefinition.Payload);
            throw new NotImplementedException();
        }
    }
}
