using CaseManagement.Common;
using CaseManagement.Common.Expression;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Jobs.Persistence;
using CaseManagement.Common.Lock;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.NotificationInstance.Commands;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Infrastructure.Jobs
{
    public class ProcessDeadLinesJob : BaseScheduledJob
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly IParameterParser _parameterParser;
        private readonly IMediator _mediator;

        public ProcessDeadLinesJob(IParameterParser parameterParser,
            IMediator mediator,
            IDistributedLock distributedLock,
            IOptions<CommonOptions> options,
            ILogger<BaseScheduledJob> logger,
            IScheduledJobStore scheduledJobStore,
            IServiceScopeFactory serviceScopeFactory) : base(distributedLock, options, logger, scheduledJobStore)
        {
            _parameterParser = parameterParser;
            _mediator = mediator;
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            _humanTaskInstanceQueryRepository = serviceProvider.GetService<IHumanTaskInstanceQueryRepository>();
            _humanTaskInstanceCommandRepository = serviceProvider.GetService<IHumanTaskInstanceCommandRepository>();
        }

        protected override string LockName => "processdeadlines";

        protected override async Task Execute(CancellationToken token)
        {
            try
            {
                var currentDateTime = DateTime.UtcNow;
                var humanTaskInstanceLst = await _humanTaskInstanceQueryRepository.GetPendingDeadLines(token, currentDateTime);
                foreach (var humanTaskInstance in humanTaskInstanceLst)
                {
                    var deadLines = humanTaskInstance.DeadLines.Where(d => currentDateTime >= d.EndDateTime);
                    var executionContext = new BaseExpressionContext(humanTaskInstance.InputParameters);
                    var deadLineIds = deadLines.Select(_ => _.Id).ToList();
                    foreach (var id in deadLineIds)
                    {
                        var deadLine = deadLines.First(_ => _.Id == id);
                        if (deadLine.Escalations != null && deadLine.Escalations.Any())
                        {
                            foreach (var escalation in deadLine.Escalations)
                            {
                                if (!string.IsNullOrWhiteSpace(escalation.Condition) && !ExpressionParser.IsValid(escalation.Condition, executionContext))
                                {
                                    continue;
                                }

                                if (!string.IsNullOrWhiteSpace(escalation.NotificationId))
                                {
                                    var parameters = _parameterParser.ParseToPartParameters(escalation.ToParts, humanTaskInstance.InputParameters);
                                    await _mediator.Send(new CreateNotificationInstanceCommand { NotificationId = escalation.NotificationId, Parameters = parameters }, token);
                                }
                            }
                        }

                        humanTaskInstance.RemoveDeadLine(deadLine.Name, deadLine.Usage);
                    }

                    await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, token);
                }

                await _humanTaskInstanceCommandRepository.SaveChanges(token);
            }
            catch(Exception)
            {

            }
        }
    }
}
