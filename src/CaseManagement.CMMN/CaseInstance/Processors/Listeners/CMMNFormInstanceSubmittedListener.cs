using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class FormInstanceSubmittedListener
    {
        private readonly PlanItemProcessorParameter _parameter;
        private readonly ManualResetEvent _manualResetEvent;

        public FormInstanceSubmittedListener(PlanItemProcessorParameter parameter, ManualResetEvent manualResetEvent)
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
            var evt = args.DomainEvt as CMMNWorkflowElementInstanceFormSubmittedEvent;
            if (evt == null)
            {
                return;
            }

            if (evt.ElementId == _parameter.WorkflowElementInstance.Id)
            {
                Unsubscribe();
                _manualResetEvent.Set();
            }
        }
    }

    public class CMMNFormInstanceSubmittedListener
    {
        public static FormInstanceSubmittedListener Listen(PlanItemProcessorParameter parameter)
        {
            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new FormInstanceSubmittedListener(parameter, manualResetEvent);
            criterionListener.Listen();
            return criterionListener;
        }
    }
}
