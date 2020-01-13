using CaseManagement.CMMN.Domains;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class CMMNManualActivationListener
    {
        public static bool Listen(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
            if (!parameter.CaseInstance.IsManualActivationRuleSatisfied(parameter.CaseElementInstance.Id, parameter.CaseDefinition))
            {
                return false;
            }
            
            parameter.CaseInstance.MakeTransitionEnable(parameter.CaseElementInstance.Id);
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
