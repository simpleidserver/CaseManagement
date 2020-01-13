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
        public class ListenEntryCriteriaResult
        {
            public ListenEntryCriteriaResult(bool isCriteriaSatisfied)
            {
                IsCriteriaSatisfied = isCriteriaSatisfied;
            }

            public ListenEntryCriteriaResult(Task task, CriterionListener listener)
            {
                IsCriteriaSatisfied = false;
                Task = task;
                Listener = listener;
            }

            public bool IsCriteriaSatisfied { get; set; }
            public Task Task { get; set; }
            public CriterionListener Listener { get; set; }
        }

        public static void ListenEntryCriterias(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
            var entryCriterion = planItemDefinition.EntryCriterions.ToList();
            if (!entryCriterion.Any())
            {
                return;
            }

            if (entryCriterion.Any(c => parameter.CaseInstance.IsCriteriaSatisfied(c, parameter.CaseElementInstance.Version)))
            {
                return;
            }

            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new CriterionListener(parameter, manualResetEvent, entryCriterion);
            criterionListener.Listen();
        }

        public static ListenEntryCriteriaResult ListenEntryCriteriasBg(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
            var entryCriterion = planItemDefinition.EntryCriterions.ToList();
            if (!entryCriterion.Any())
            {
                return null;
            }

            if (entryCriterion.Any(c => parameter.CaseInstance.IsCriteriaSatisfied(c, parameter.CaseElementInstance.Version)))
            {
                return new ListenEntryCriteriaResult(true);
            }

            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new CriterionListener(parameter, manualResetEvent, entryCriterion);
            var task = new Task(() =>
            {
                criterionListener.Listen();
            }, TaskCreationOptions.LongRunning);
            task.Start();
            return new ListenEntryCriteriaResult(task, criterionListener);
        }

        public static KeyValuePair<Task, CriterionListener>? ListenExitCriterias(ProcessorParameter parameter)
        {
            var planItemDefinition = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId);
            return ListenExitCriterias(parameter, planItemDefinition.ExitCriterions.ToList());
        }

        public static KeyValuePair<Task, CriterionListener>? ListenExitCriterias(ProcessorParameter parameter, ICollection<Criteria> criterias)
        {
            if (!criterias.Any())
            {
                return null;
            }

            if (criterias.Any(c => parameter.CaseInstance.IsCriteriaSatisfied(c, parameter.CaseElementInstance.Version)))
            {
                throw new TerminateCaseInstanceElementException();
            }

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
            private readonly ICollection<Criteria> _criterions;

            public CriterionListener(ProcessorParameter parameter, ManualResetEvent manualResetEvent, ICollection<Criteria> criterions)
            {
                _parameter = parameter;
                _manualResetEvent = manualResetEvent;
                _criterions = criterions;
            }

            public void Listen()
            {
                _parameter.CaseInstance.EventRaised += HandlePlanItemChanged;
                _manualResetEvent.WaitOne();
            }

            public void Unsubscribe()
            {
                _parameter.CaseInstance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object obj, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CaseElementTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }

                var sourcePlanItemInstance = _parameter.CaseInstance.GetWorkflowElementInstance(evt.CaseElementId);
                if (!_criterions.Any(e => e.SEntry.PlanItemOnParts.Any(p => p.SourceRef == sourcePlanItemInstance.CaseElementDefinitionId && p.StandardEvent == evt.Transition)))
                {
                    return;
                }

                if (_criterions.Any(c => _parameter.CaseInstance.IsCriteriaSatisfied(c, _parameter.CaseElementInstance.Version)))
                {
                    Unsubscribe();
                    _manualResetEvent.Set();
                }
            }
        }
    }
}