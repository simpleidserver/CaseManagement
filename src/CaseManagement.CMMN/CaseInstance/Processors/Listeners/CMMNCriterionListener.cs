using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class CMMNCriterionListener
    {
        public static void Listen(PlanItemProcessorParameter parameter)
        {
            var planItemDefinition = parameter.WorkflowDefinition.Elements.First(p => p.Id == parameter.WorkflowElementInstance.WorkflowElementDefinitionId);
            if (!planItemDefinition.EntryCriterions.Any())
            {
                return;
            }


            if (planItemDefinition.EntryCriterions.Any(c => parameter.WorkflowInstance.IsCriteriaSatisfied(c, parameter.WorkflowElementInstance.Version)))
            {
                return;
            }

            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new CriterionListener(parameter, manualResetEvent);
            criterionListener.Listen();
        }

        public class CriterionListener
        {
            private readonly PlanItemProcessorParameter _parameter;
            private readonly ManualResetEvent _manualResetEvent;

            public CriterionListener(PlanItemProcessorParameter parameter, ManualResetEvent manualResetEvent)
            {
                _parameter = parameter;
                _manualResetEvent = manualResetEvent;
            }

            public void Listen()
            {
                _parameter.WorkflowInstance.EventRaised += HandlePlanItemChanged;
                _manualResetEvent.WaitOne();
            }

            public void Unsubscribe()
            {
                _parameter.WorkflowInstance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object obj, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CMMNWorkflowElementTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }
                
                var sourcePlanItemInstance = _parameter.WorkflowInstance.GetWorkflowElementInstance(evt.ElementId);
                var planItemDefinition = _parameter.WorkflowDefinition.Elements.First(p => p.Id == _parameter.WorkflowElementInstance.WorkflowElementDefinitionId);
                if (!planItemDefinition.EntryCriterions.Any(e => e.SEntry.PlanItemOnParts.Any(p => p.SourceRef == sourcePlanItemInstance.WorkflowElementDefinitionId && p.StandardEvent == evt.Transition)))
                {
                    return;
                }

                if (planItemDefinition.EntryCriterions.Any(c => _parameter.WorkflowInstance.IsCriteriaSatisfied(c, _parameter.WorkflowElementInstance.Version)))
                {
                    Unsubscribe();
                    _manualResetEvent.Set();
                }
            }
        }
    }
}