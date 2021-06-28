using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class ReactivateCommandHandler : BaseExternalEventNotification, IRequestHandler<ReactivateCommand, bool>
    {
        public ReactivateCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Reactivate;

        public Task<bool> Handle(ReactivateCommand command, CancellationToken token)
        {
            return Execute(command.CaseInstanceId, command.CaseInstanceElementId, command.Parameters, token);
        }
    }
}
