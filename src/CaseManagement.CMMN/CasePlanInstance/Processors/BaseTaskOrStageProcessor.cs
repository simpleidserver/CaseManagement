using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public abstract class BaseTaskOrStageProcessor<T> : BaseProcessor<T> where T : BaseTaskOrStageElementInstance
    {
        public BaseTaskOrStageProcessor(ISubscriberRepository subscriberRepository) : base(subscriberRepository) { }

        public override async Task Execute(ExecutionContext executionContext, T elt, CancellationToken cancellationToken)
        {
            if (elt.State == null)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, Domains.CMMNTransitions.Create);
            }

            if (elt.State == Domains.TaskStageStates.Available)
            {
                if (elt.ManualActivationRule != null && executionContext.CasePlanInstance.IsManualActivationRuleSatisfied())
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, Domains.CMMNTransitions.Enable);
                    return;
                }

                executionContext.CasePlanInstance.MakeTransition(elt, Domains.CMMNTransitions.Start);
            }

            if (elt.State == Domains.TaskStageStates.Active)
            {
                await ProtectedExecute(executionContext, elt, cancellationToken);
            }
        }

        protected abstract Task ProtectedExecute(ExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
