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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        public ProcessDeadLinesJob(IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            IParameterParser parameterParser,
            IMediator mediator,
            IDistributedLock distributedLock,
            IOptions<CommonOptions> options,
            ILogger<BaseScheduledJob> logger,
            IScheduledJobStore scheduledJobStore) : base(distributedLock, options, logger, scheduledJobStore)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _parameterParser = parameterParser;
            _mediator = mediator;
        }

        protected override string LockName => "processdeadlines";

        protected override async Task Execute(CancellationToken token)
        {
            var humanTaskInstanceLst = await _humanTaskInstanceQueryRepository.GetPendingDeadLines(token);
            foreach(var humanTaskInstance in humanTaskInstanceLst)
            {
                var executionContext = new BaseExpressionContext(humanTaskInstance.OperationParameters);
                foreach(var deadLine in humanTaskInstance.DeadLines)
                {
                    if (deadLine.Escalations != null && deadLine.Escalations.Any())
                    {
                        foreach(var escalation in deadLine.Escalations)
                        {
                            if (!string.IsNullOrWhiteSpace(escalation.Condition) && !ExpressionParser.IsValid(escalation.Condition, executionContext))
                            {
                                continue;
                            }

                            if (escalation.Notification != null)
                            {
                                var parameters = _parameterParser.ParseToPartParameters(escalation.ToParts, humanTaskInstance.OperationParameters);
                                await _mediator.Send(new CreateNotificationInstanceCommand { NotificationDef = escalation.Notification, Parameters = parameters }, token);
                            }
                        }
                    }

                    humanTaskInstance.RemoveDeadLine(deadLine.Name, deadLine.Type);
                }

                await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, token);
            }

            await _humanTaskInstanceCommandRepository.SaveChanges(token);
        }
    }
}
