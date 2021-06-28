using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using MassTransit;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.EventHandlers
{
    public class CaseInstanceWorkerTaskAddedEventConsumer : IConsumer<CaseInstanceWorkerTaskAddedEvent>
    {
        private readonly ICaseWorkerTaskCommandRepository _caseWorkerTaskCommandRepository;

        public CaseInstanceWorkerTaskAddedEventConsumer(ICaseWorkerTaskCommandRepository caseWorkerTaskCommandRepository)
        {
            _caseWorkerTaskCommandRepository = caseWorkerTaskCommandRepository;
        }

        public async Task Consume(ConsumeContext<CaseInstanceWorkerTaskAddedEvent> context)
        {
            var caseWorkerTask = CaseWorkerTaskAggregate.New(context.Message);
            await _caseWorkerTaskCommandRepository.Add(caseWorkerTask, CancellationToken.None);
            await _caseWorkerTaskCommandRepository.SaveChanges(CancellationToken.None);
        }
    }
}
