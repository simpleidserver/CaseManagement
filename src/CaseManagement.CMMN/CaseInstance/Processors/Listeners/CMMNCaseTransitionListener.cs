using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public static class CMMNCaseTransitionListener
    {
        public static EventListener Listen(CMMNWorkflowInstance instance, CMMNTransitions transition, Action callback)
        {
            var evtListener = new EventListener(instance, transition, callback);
            evtListener.Listen();
            return evtListener;
        }

        public class EventListener
        {
            private readonly CMMNWorkflowInstance _instance;
            private readonly CMMNTransitions _transition;
            private readonly Action _callback;

            public EventListener(CMMNWorkflowInstance instance, CMMNTransitions transition, Action callback)
            {
                _instance = instance;
                _transition = transition;
                _callback = callback;
            }

            public void Listen()
            {
                _instance.EventRaised += HandlePlanItemChanged;
            }

            public void Unsubscribe()
            {
               _instance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object sender, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CMMNWorkflowTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }

                if (evt.AggregateId == _instance.Id && evt.Transition == _transition)
                {
                    _callback();
                    Unsubscribe();
                }
            }
        }
    }
}
