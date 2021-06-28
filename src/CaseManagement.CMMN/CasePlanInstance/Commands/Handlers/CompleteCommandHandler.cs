using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class CompleteCommandHandler : BaseExternalEventNotification, IRequestHandler<CompleteCommand, bool>
    {
        public CompleteCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Complete;

        public Task<bool> Handle(CompleteCommand completeCommand, CancellationToken token)
        {
            return Execute(completeCommand.CaseInstanceId, completeCommand.CaseInstanceElementId, completeCommand.Parameters, token);
        }
    }
}
