using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class CMMNCriterionListener
    {
        public static void ListenEntryCriterias(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.WorkflowDefinition.GetElement(parameter.WorkflowElementInstance.WorkflowElementDefinitionId);
            if (!planItemDefinition.EntryCriterions.Any())
            {
                return;
            }


            if (planItemDefinition.EntryCriterions.Any(c => parameter.WorkflowInstance.IsCriteriaSatisfied(c, parameter.WorkflowElementInstance.Version)))
            {
                return;
            }

            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new CriterionListener(parameter, manualResetEvent, planItemDefinition.EntryCriterions);
            criterionListener.Listen();
        }

        public static KeyValuePair<Task, CriterionListener>? ListenEntryCriteriasBg(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.WorkflowDefinition.GetElement(parameter.WorkflowElementInstance.WorkflowElementDefinitionId);
            if (!planItemDefinition.EntryCriterions.Any())
            {
                return null;
            }


            if (planItemDefinition.EntryCriterions.Any(c => parameter.WorkflowInstance.IsCriteriaSatisfied(c, parameter.WorkflowElementInstance.Version)))
            {
                return null;
            }

            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new CriterionListener(parameter, manualResetEvent, planItemDefinition.EntryCriterions);
            var task = new Task(() =>
            {
                criterionListener.Listen();
            }, TaskCreationOptions.LongRunning);
            task.Start();
            return new KeyValuePair<Task, CriterionListener>(task, criterionListener);
        }

        public static KeyValuePair<Task, CriterionListener>? ListenExitCriterias(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.WorkflowDefinition.GetElement(parameter.WorkflowElementInstance.WorkflowElementDefinitionId);
            if (!planItemDefinition.ExitCriterions.Any())
            {
                return null;
            }

            if (planItemDefinition.ExitCriterions.Any(c => parameter.WorkflowInstance.IsCriteriaSatisfied(c, parameter.WorkflowElementInstance.Version)))
            {
                throw new TerminateCaseInstanceElementException();
            }

            return ListenExitCriterias(parameter, planItemDefinition.ExitCriterions);
        }

        public static KeyValuePair<Task, CriterionListener>? ListenExitCriterias(ProcessorParameter parameter, ICollection<CMMNCriterion> criterias)
        {
            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new CriterionListener(parameter, manualResetEvent, criterias);
            var task = new Task(() =>
            {
                criterionListener.Listen();
            }, TaskCreationOptions.LongRunning);
            task.Start();
            return new KeyValuePair<Task, CriterionListener>(task, criterionListener);
        }

        public class CriterionListener
        {
            private readonly ProcessorParameter _parameter;
            private readonly ManualResetEvent _manualResetEvent;
            private readonly ICollection<CMMNCriterion> _criterions;

            public CriterionListener(ProcessorParameter parameter, ManualResetEvent manualResetEvent, ICollection<CMMNCriterion> criterions)
            {
                _parameter = parameter;
                _manualResetEvent = manualResetEvent;
                _criterions = criterions;
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
                if (!_criterions.Any(e => e.SEntry.PlanItemOnParts.Any(p => p.SourceRef == sourcePlanItemInstance.WorkflowElementDefinitionId && p.StandardEvent == evt.Transition)))
                {
                    return;
                }

                if (_criterions.Any(c => _parameter.WorkflowInstance.IsCriteriaSatisfied(c, _parameter.WorkflowElementInstance.Version)))
                {
                    Unsubscribe();
                    _manualResetEvent.Set();
                }
            }
        }
    }
}