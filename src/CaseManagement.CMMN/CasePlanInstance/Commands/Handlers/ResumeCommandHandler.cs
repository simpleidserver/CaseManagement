using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class ResumeCommandHandler : BaseExternalEventNotification, IRequestHandler<ResumeCommand, bool>
    {
        public ResumeCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.Resume;

        public Task<bool> Handle(ResumeCommand command, CancellationToken token)
        {
            return Execute(command.CasePlanInstanceId, command.CasePlanInstanceElementId, command.Parameters, token);
        }
    }
}
