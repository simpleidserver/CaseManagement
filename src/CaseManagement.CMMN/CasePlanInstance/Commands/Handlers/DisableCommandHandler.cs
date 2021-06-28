using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class DisableCommandHandler : BaseExternalEventNotification, IRequestHandler<DisableCommand, bool>
    {
        public DisableCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Disable;

        public Task<bool> Handle(DisableCommand command, CancellationToken token)
        {
            return Execute(command.CasePlanInstanceId, command.CasePlanElementInstanceId, command.Parameters, token);
        }
    }
}