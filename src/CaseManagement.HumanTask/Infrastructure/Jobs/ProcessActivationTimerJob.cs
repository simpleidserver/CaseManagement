using CaseManagement.Common;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.Common.Lock;
using CaseManagement.HumanTask.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CaseManagement.HumanTask.Infrastructure.Jobs
{
    public class ProcessActivationTimerJob : BaseScheduledJob
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;

        public ProcessActivationTimerJob(IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IDistributedLock distributedLock, 
            IOptions<CommonOptions> options, 
            ILogger<BaseScheduledJob> logger, 
            IScheduledJobStore scheduledJobStore) : base(distributedLock, options, logger, scheduledJobStore)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
        }

        protected override string LockName => "processactivationtimer";

        protected override async Task Execute(CancellationToken token)
        {
            var humanTaskListInstances = await _humanTaskInstanceQueryRepository.GetPendingLst(token);
            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var instance in humanTaskListInstances)
                {
                    instance.Activate("ProcessActivationTimer");
                    await _humanTaskInstanceCommandRepository.Update(instance, token);
                    await _humanTaskInstanceCommandRepository.SaveChanges(token);
                }

                transaction.Complete();
            }
        }
    }
}
