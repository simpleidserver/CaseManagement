using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class OccurCommandHandler : BaseExternalEventNotification, IRequestHandler<OccurCommand, bool>
    {
        public OccurCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Occur;

        public Task<bool> Handle(OccurCommand command, CancellationToken token)
        {
            return Execute(command.CasePlanInstanceId, command.CasePlanElementInstanceId, command.Parameters, token);
        }
    }
}
