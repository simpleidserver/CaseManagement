using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class TerminateCommandHandler : BaseExternalEventNotification, IRequestHandler<TerminateCommand, bool>
    {
        public TerminateCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Terminate;

        public Task<bool> Handle(TerminateCommand command, CancellationToken token)
        {
            return Execute(command.CaseInstanceId, command.CaseInstanceElementId, command.Parameters, token);
        }
    }
}
