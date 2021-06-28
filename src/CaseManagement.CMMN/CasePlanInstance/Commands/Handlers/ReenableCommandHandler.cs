using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class ReenableCommandHandler : BaseExternalEventNotification, IRequestHandler<ReenableCommand, bool>
    {
        public ReenableCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Reenable;

        public Task<bool> Handle(ReenableCommand command, CancellationToken token)
        {
            return Execute(command.CasePlanInstanceId, command.CasePlanElementInstanceId, command.Parameters, token);
        }
    }
}
