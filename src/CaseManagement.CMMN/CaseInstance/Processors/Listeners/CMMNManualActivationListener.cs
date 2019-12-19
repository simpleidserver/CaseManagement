using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using System.Linq;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class CMMNManualActivationListener
    {
        public static bool Listen(PlanItemProcessorParameter parameter)
        {
            var planItemDefinition = parameter.WorkflowDefinition.PlanItems.FirstOrDefault(p => p.Id == parameter.PlanItemInstance.PlanItemDefinitionId);
            if (planItemDefinition.ManualActivationRule == null || (planItemDefinition.ManualActivationRule.Expression != null && !ExpressionParser.IsValid(planItemDefinition.ManualActivationRule.Expression.Body, parameter.WorkflowInstance)))
            {
                return false;
            }

            var semaphore = new Semaphore(0, 1);
            var manualActivation = new ManualActivationListener(parameter, semaphore);
            manualActivation.Listen();
            semaphore.WaitOne();
            return true;
        }
        
        public class ManualActivationListener
        {
            private readonly PlanItemProcessorParameter _parameter;
            private readonly Semaphore _semaphore;

            public ManualActivationListener(PlanItemProcessorParameter parameter, Semaphore semaphore)
            {
                _parameter = parameter;
                _semaphore = semaphore;
            }

            public void Listen()
            {
                _parameter.WorkflowInstance.EventRaised += HandlePlanItemChanged;
                _semaphore.WaitOne();
            }
            
            public void Unsubscribe()
            {
                _parameter.WorkflowInstance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object sender, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CMMNPlanItemTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }

                if (evt.ElementId == _parameter.PlanItemInstance.Id && evt.Transition == CMMNPlanItemTransitions.ManualStart)
                {
                    _semaphore.Release();
                }
            }
        }
    }
}
