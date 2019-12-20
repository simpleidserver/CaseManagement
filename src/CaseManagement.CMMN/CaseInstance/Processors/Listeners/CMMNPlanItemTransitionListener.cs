using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public static class CMMNPlanItemTransitionListener
    {
        public static EventListener Listen(PlanItemProcessorParameter parameter, CMMNTransitions transition, Action callback)
        {
            var evtListener = new EventListener(parameter, transition, callback);
            evtListener.Listen();
            return evtListener;
        }

        public class EventListener
        {
            private readonly PlanItemProcessorParameter _parameter;
            private readonly CMMNTransitions _transition;
            private readonly Action _callback;

            public EventListener(PlanItemProcessorParameter parameter, CMMNTransitions transition, Action callback)
            {
                _parameter = parameter;
                _transition = transition;
                _callback = callback;
            }

            public void Listen()
            {
                _parameter.WorkflowInstance.EventRaised += HandlePlanItemChanged;
            }

            public void Unsubscribe()
            {
                _parameter.WorkflowInstance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object sender, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CMMNWorkflowElementTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }

                if (evt.ElementId == _parameter.WorkflowElementInstance.Id && evt.Transition == _transition)
                {
                    _callback();
                    Unsubscribe();
                }
            }
        }
    }
}
