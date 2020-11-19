using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Parser;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.BPMN.Resources;
using CaseManagement.Common;
using CaseManagement.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Commands.Handlers
{
    public class CreateProcessInstanceCommandHandler : IRequestHandler<CreateProcessInstanceCommand, SearchResult<ProcessInstanceResult>>
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

        public async Task<SearchResult<ProcessInstanceResult>> Handle(CreateProcessInstanceCommand request, CancellationToken cancellationToken)
        {
            var workflowDefinition = await _processFileQueryRepository.Get(request.ProcessFileId, cancellationToken);
            if (workflowDefinition == null)
            {
                _logger.LogError($"the process file '{request.ProcessFileId}' doesn't exist");
                throw new UnknownProcessFileException(string.Format(Global.UnknownProcessFile, request.ProcessFileId));
            }

            var processInstances = BPMNParser.BuildInstances(workflowDefinition.Payload, request.ProcessFileId);
            var result = new List<ProcessInstanceResult>();
            foreach(var processInstance in processInstances)
            {
                var pi = ProcessInstanceAggregate.New(request.ProcessFileId,
                    processInstance.ElementDefs.ToList(),
                    processInstance.Interfaces.ToList(),
                    processInstance.Messages.ToList(),
                    processInstance.ItemDefs.ToList(),
                    processInstance.SequenceFlows.ToList());
                await _commitAggregateHelper.Commit(pi, pi.GetStreamName(), cancellationToken);
                result.Add(ProcessInstanceResult.ToDto(pi));
            }

            return new SearchResult<ProcessInstanceResult>
            {
                Content = result
            };
        }
    }
}
