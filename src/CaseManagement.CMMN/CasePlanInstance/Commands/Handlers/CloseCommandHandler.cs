using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class CloseCommandHandler : BaseExternalEventNotification, IRequestHandler<CloseCommand, bool>
    {
        public CloseCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Close;

        public Task<bool> Handle(CloseCommand closeCommand, CancellationToken token)
        {
            return Execute(closeCommand.CasePlanInstanceId, null, closeCommand.Parameters, token);
        }
    }
}
