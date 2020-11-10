using CaseManagement.Common;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.Common.Lock;
using CaseManagement.HumanTask.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CaseManagement.HumanTask.Infrastructure.Jobs
{
    public class ProcessActivationTimerJob : BaseScheduledJob
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;

        public ProcessActivationTimerJob(IServiceScopeFactory serviceScopeFactory,
            IDistributedLock distributedLock, 
            IOptions<CommonOptions> options, 
            ILogger<BaseScheduledJob> logger, 
            IScheduledJobStore scheduledJobStore) : base(distributedLock, options, logger, scheduledJobStore)
        {
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            _humanTaskInstanceQueryRepository = serviceProvider.GetService<IHumanTaskInstanceQueryRepository>();
            _humanTaskInstanceCommandRepository = serviceProvider.GetService<IHumanTaskInstanceCommandRepository>();
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
