using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Parser;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.BPMN.Resources;
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
        private readonly IProcessInstanceCommandRepository _processInstanceCommandRepository;
        private readonly IProcessFileCommandRepository _processFileCommandRepository;
        private readonly ILogger<CreateProcessInstanceCommandHandler> _logger;

        public CreateProcessInstanceCommandHandler(
            IProcessInstanceCommandRepository processInstanceCommandRepository,
            IProcessFileCommandRepository processFileCommandRepository,
            ILogger<CreateProcessInstanceCommandHandler> logger)
        {
            _processInstanceCommandRepository = processInstanceCommandRepository;
            _processFileCommandRepository = processFileCommandRepository;
            _logger = logger;
        }

        public async Task<SearchResult<ProcessInstanceResult>> Handle(CreateProcessInstanceCommand request, CancellationToken cancellationToken)
        {
            var processFile = await _processFileCommandRepository.Get(request.ProcessFileId, cancellationToken);
            if (processFile == null || string.IsNullOrWhiteSpace(processFile.AggregateId))
            {
                _logger.LogError($"the process file '{request.ProcessFileId}' doesn't exist");
                throw new UnknownProcessFileException(string.Format(Global.UnknownProcessFile, request.ProcessFileId));
            }

            var processInstances = BPMNParser.BuildInstances(processFile.Payload, request.ProcessFileId);
            var result = new List<ProcessInstanceResult>();
            foreach(var processInstance in processInstances)
            {
                var pi = ProcessInstanceAggregate.New(request.ProcessFileId,
                    processFile.Name,
                    processInstance.ElementDefs.ToList(),
                    processInstance.Interfaces.ToList(),
                    processInstance.Messages.ToList(),
                    processInstance.ItemDefs.ToList(),
                    processInstance.SequenceFlows.ToList());
                await _processInstanceCommandRepository.Add(pi, cancellationToken);
                result.Add(ProcessInstanceResult.ToDto(pi));
            }

            await _processInstanceCommandRepository.SaveChanges(cancellationToken);
            return new SearchResult<ProcessInstanceResult>
            {
                Content = result
            };
        }
    }
}
