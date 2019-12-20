using CaseManagement.CMMN.Domains;
using System.Linq;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class CMMNManualActivationListener
    {
        public static bool Listen(PlanItemProcessorParameter parameter)
        {
            var planItemDefinition = parameter.WorkflowDefinition.Elements.FirstOrDefault(p => p.Id == parameter.WorkflowElementInstance.WorkflowElementDefinitionId);
            if (!parameter.WorkflowInstance.IsManualActivationRuleSatisfied(parameter.WorkflowElementInstance.Id, parameter.WorkflowDefinition))
            {
                return false;
            }
            
            parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Enable);
            var resetEvent = new ManualResetEvent(false);
            var manualStartListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ManualStart, () =>
            {
                resetEvent.Set();
            });

            resetEvent.WaitOne();
            manualStartListener.Unsubscribe();
            return true;
        }
    }
}
