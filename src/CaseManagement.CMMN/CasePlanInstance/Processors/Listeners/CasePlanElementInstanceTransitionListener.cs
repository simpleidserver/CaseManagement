using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Listeners
{
    public static class CasePlanElementInstanceTransitionListener
    {
        public static EventListener Listen(ProcessorParameter parameter, CMMNTransitions transition, Action callback)
        {
            var evtListener = new EventListener(parameter, transition, callback);
            evtListener.Listen();
            return evtListener;
        }

        public class EventListener
        {
            private readonly ProcessorParameter _parameter;
            private readonly CMMNTransitions _transition;
            private readonly Action _callback;

            public EventListener(ProcessorParameter parameter, CMMNTransitions transition, Action callback)
            {
                _parameter = parameter;
                _transition = transition;
                _callback = callback;
            }

            public void Listen()
            {
                _parameter.CaseInstance.EventRaised += HandlePlanItemChanged;
            }

            public void Unsubscribe()
            {
                _parameter.CaseInstance.EventRaised -= HandlePlanItemChanged;
            }

            private void HandlePlanItemChanged(object sender, DomainEventArgs args)
            {
                var evt = args.DomainEvt as CaseElementTransitionRaisedEvent;
                if (evt == null)
                {
                    return;
                }

                if (evt.CaseElementId == _parameter.CaseElementInstance.Id && evt.Transition == _transition)
                {
                    _callback();
                    Unsubscribe();
                }
            }
        }
    }
}
