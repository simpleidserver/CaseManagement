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
            var planItemDefinition = parameter.WorkflowDefinition.PlanItems.First(p => p.Id == parameter.PlanItemInstance.PlanItemDefinitionId);
            if (!planItemDefinition.EntryCriterions.Any())
            {
                return;
            }

            var semaphore = new Semaphore(planItemDefinition.EntryCriterions.Count() - 1, planItemDefinition.EntryCriterions.Count());
            var listeners = new List<CriterionListener>();
            foreach (var entryCriterion in planItemDefinition.EntryCriterions)
            {
                var listener = new CriterionListener(parameter, entryCriterion, semaphore);
                listener.Listen();
                listeners.Add(listener);
                semaphore.WaitOne();
            }

            foreach(var listener in listeners)
            {
                listener.Unsubscribe();
            }
        }

        public class CriterionListener
        {
            private Dictionary<string, CMMNPlanItemTransitions> _mappingPlanItemEvent;
            private readonly PlanItemProcessorParameter _parameter;
            private readonly CMMNCriterion _criterion;
            private readonly Semaphore _semaphore;

            public CriterionListener(PlanItemProcessorParameter parameter, CMMNCriterion criterion, Semaphore semaphore)
            {
                _parameter = parameter;
                _criterion = criterion;
                _semaphore = semaphore;
            }

            public void Listen()
            {
                _parameter.WorkflowInstance.EventRaised += HandlePlanItemChanged;
                _mappingPlanItemEvent = new Dictionary<string, CMMNPlanItemTransitions>();
                var planItemOnParts = _criterion.SEntry.PlanItemOnParts.Where(p => !string.IsNullOrWhiteSpace(p.SourceRef));
                foreach (var planItemOnPart in planItemOnParts)
                {
                    _mappingPlanItemEvent.Add(planItemOnPart.SourceRef, planItemOnPart.StandardEvent);
                }
            }

            public void Unsubscribe()
            {
                _parameter.WorkflowInstance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object obj, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CMMNPlanItemTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }

                var planItemInstance = _parameter.WorkflowInstance.GetPlanItemInstance(evt.ElementId);
                if (_mappingPlanItemEvent.ContainsKey(planItemInstance.PlanItemDefinitionId))
                {
                    var standardEvent = _mappingPlanItemEvent[planItemInstance.PlanItemDefinitionId];
                    if (evt.Transition == standardEvent)
                    {
                        _mappingPlanItemEvent.Remove(evt.ElementId);
                        _semaphore.Release();
                    }
                }
            }
        }
    }
}