using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.EventHandlers
{
    public class CaseInstanceWorkerTaskRemovedEventConsumer : IConsumer<CaseInstanceWorkerTaskRemovedEvent>
    {
        private readonly ICaseWorkerTaskCommandRepository _caseWorkerTaskCommandRepository;

        public CaseInstanceWorkerTaskRemovedEventConsumer(ICaseWorkerTaskCommandRepository caseWorkerTaskCommandRepository)
        {
            _caseWorkerTaskCommandRepository = caseWorkerTaskCommandRepository;
        }

        public async Task Consume(ConsumeContext<CaseInstanceWorkerTaskRemovedEvent> context)
        {
            var id = CaseWorkerTaskAggregate.BuildCaseWorkerTaskIdentifier(context.Message.AggregateId, context.Message.CasePlanInstanceElementId);
            var result = await _caseWorkerTaskCommandRepository.Get(id, CancellationToken.None);
            await _caseWorkerTaskCommandRepository.Delete(result, CancellationToken.None);
            await _caseWorkerTaskCommandRepository.SaveChanges(CancellationToken.None);
        }
    }
}
