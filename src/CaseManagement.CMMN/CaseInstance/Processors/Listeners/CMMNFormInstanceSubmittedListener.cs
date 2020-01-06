using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System.Diagnostics;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class FormInstanceSubmittedListener
    {
        private readonly CMMNWorkflowInstance _workflowInstance;
        private readonly CMMNWorkflowElementInstance _workflowElementInstance;
        private readonly ManualResetEvent _manualResetEvent;

        public FormInstanceSubmittedListener(CMMNWorkflowInstance workflowInstance, CMMNWorkflowElementInstance workflowElementInstance, ManualResetEvent manualResetEvent)
        {
            _workflowInstance = workflowInstance;
            _workflowElementInstance = workflowElementInstance;
            _manualResetEvent = manualResetEvent;
        }

        public void Listen()
        {
            _workflowInstance.EventRaised += HandleFormInstanceSubmitted;
            _manualResetEvent.WaitOne();
        }

        public void Unsubscribe()
        {
            _workflowInstance.EventRaised -= HandleFormInstanceSubmitted;
        }

        private void HandleFormInstanceSubmitted(object obj, DomainEventArgs args)
        {
            var evt = args.DomainEvt as CMMNWorkflowElementInstanceFormSubmittedEvent;
            if (evt == null)
            {
                return;
            }

            if (evt.ElementId == _workflowElementInstance.Id)
            {
                Unsubscribe();
                _manualResetEvent.Set();
            }
        }
    }

    public class CMMNFormInstanceSubmittedListener
    {
        public static FormInstanceSubmittedListener Listen(ProcessorParameter parameter)
        {
            var manualResetEvent = new ManualResetEvent(false);
            var criterionListener = new FormInstanceSubmittedListener(parameter.WorkflowInstance, parameter.WorkflowElementInstance, manualResetEvent);
            criterionListener.Listen();
            return criterionListener;
        }
    }
}
