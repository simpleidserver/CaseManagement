using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class SuspendCommandHandler : BaseExternalEventNotification, IRequestHandler<SuspendCommand, bool>
    {
        public SuspendCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Suspend;

        public Task<bool> Handle(SuspendCommand command, CancellationToken token)
        {
            return Execute(command.CasePlanInstanceId, command.CasePlanInstanceElementId, command.Parameters, token);
        }
    }
}
