using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class AddChildCommandHandler : BaseExternalEventNotification, IRequestHandler<AddChildCommand, bool>
    {
        public AddChildCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.AddChild;

        public Task<bool> Handle(AddChildCommand addChildCommand, CancellationToken token)
        {
            return Execute(addChildCommand.CasePlanInstanceId, addChildCommand.CasePlanInstanceElementId, addChildCommand.Parameters, token);
        }
    }
}
