using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Domains.ProcessDefinition.Events;
using CaseManagement.BPMN.Persistence;
using CaseManagement.Common.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile
{
    public class ProcessFileHandler : 
        IDomainEvtConsumerGeneric<ProcessFileAddedEvent>,
        IDomainEvtConsumerGeneric<ProcessFileUpdatedEvent>,
        IDomainEvtConsumerGeneric<ProcessFilePublishedEvent>
    {
        private readonly IProcessFileCommandRepository _processFileCommandRepository;
        private readonly IProcessFileQueryRepository _processFileQueryRepository;

        public ProcessFileHandler(IProcessFileCommandRepository processFileCommandRepository, IProcessFileQueryRepository processFileQueryRepository)
        {
            _processFileCommandRepository = processFileCommandRepository;
            _processFileQueryRepository = processFileQueryRepository;
        }

        public async Task Handle(ProcessFileAddedEvent message, CancellationToken token)
        {
            var caseWorkerTask = ProcessFileAggregate.New(new DomainEvent[] { message });
            await _processFileCommandRepository.Add(caseWorkerTask, token);
            await _processFileCommandRepository.SaveChanges(token);
        }

        public async Task Handle(ProcessFileUpdatedEvent message, CancellationToken token)
        {
            var processFile = await _processFileQueryRepository.Get(message.AggregateId, token);
            processFile.Handle(message);
            await _processFileCommandRepository.Update(processFile, token);
            await _processFileCommandRepository.SaveChanges(token);
        }

        public async Task Handle(ProcessFilePublishedEvent message, CancellationToken token)
        {
            var processFile = await _processFileQueryRepository.Get(message.AggregateId, token);
            processFile.Handle(message);
            await _processFileCommandRepository.Update(processFile, token);
            await _processFileCommandRepository.SaveChanges(token);
        }
    }
}
