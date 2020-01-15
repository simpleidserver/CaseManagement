using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.Events;
using System.Threading;

namespace CaseManagement.CMMN.CaseInstance.Processors.Listeners
{
    public class FormInstanceSubmittedListener
    {
        private readonly Domains.CaseInstance _workflowInstance;
        private readonly CaseElementInstance _workflowElementInstance;
        private readonly CancellationToken _cancellationToken;
        private bool _continueExecution;

        public FormInstanceSubmittedListener(Domains.CaseInstance workflowInstance, CaseElementInstance workflowElementInstance, CancellationToken cancellationToken)
        {
            _workflowInstance = workflowInstance;
            _workflowElementInstance = workflowElementInstance;
            _cancellationToken = cancellationToken;
        }

        public void Listen()
        {
            _continueExecution = true;
            _workflowInstance.EventRaised += HandleFormInstanceSubmitted;
            while (_continueExecution)
            {
                Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                if (_cancellationToken.IsCancellationRequested)
                {
                    _continueExecution = false;
                }
            }
        }

        public void Unsubscribe()
        {
            _workflowInstance.EventRaised -= HandleFormInstanceSubmitted;
            _continueExecution = false;
        }

        private void HandleFormInstanceSubmitted(object obj, DomainEventArgs args)
        {
            var evt = args.DomainEvt as CaseElementInstanceFormSubmittedEvent;
            if (evt == null)
            {
                return;
            }

            if (evt.CaseElementId == _workflowElementInstance.Id)
            {
                Unsubscribe();
            }
        }
    }

    public class CMMNFormInstanceSubmittedListener
    {
        public static FormInstanceSubmittedListener Listen(ProcessorParameter parameter, CancellationToken token)
        {
            var criterionListener = new FormInstanceSubmittedListener(parameter.CaseInstance, parameter.CaseElementInstance, token);
            criterionListener.Listen();
            return criterionListener;
        }
    }
}
