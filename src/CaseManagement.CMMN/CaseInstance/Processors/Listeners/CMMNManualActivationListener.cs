using CaseManagement.CMMN.Domains;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class CMMNManualActivationListener
    {
        public static bool Listen(ProcessorParameter parameter, CancellationToken cancellationToken)
        {
            var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
            if (!parameter.CaseInstance.IsManualActivationRuleSatisfied(parameter.CaseElementInstance.Id, parameter.CaseDefinition))
            {
                return false;
            }
            
            parameter.CaseInstance.MakeTransitionEnable(parameter.CaseElementInstance.Id);
            bool continueExecution = true;
            var manualStartListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ManualStart, () =>
            {
                continueExecution = false;
            });
            while(continueExecution)
            {
                Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                if (cancellationToken.IsCancellationRequested)
                {
                    continueExecution = false;
                }
            }

            manualStartListener.Unsubscribe();
            return true;
        }
    }
}
