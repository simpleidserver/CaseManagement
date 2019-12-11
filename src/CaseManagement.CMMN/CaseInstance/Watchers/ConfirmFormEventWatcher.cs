using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public class ConfirmFormEventWatcher : IConfirmFormEventWatcher
    {
        private bool _quit;
        private CancellationToken _token;
        private WorkflowHandlerContext _context;

        public ConfirmFormEventWatcher() { }

        public Task Task { get; private set; }

        public Task Start(WorkflowHandlerContext context, CancellationToken token)
        {
            _quit = false;
            _token = token;
            _context = context;
            Task = new Task(Handle, token, TaskCreationOptions.LongRunning);
            Task.Start();
            return Task.CompletedTask;
        }

        private void Handle()
        {
            _context.ProcessFlowInstance.EventRaised += HandleEventRaised;
            while(!_token.IsCancellationRequested)
            {
                Task.Delay(10).Wait();
                if (_quit)
                {
                    return;
                }
            }

            _context.ProcessFlowInstance.EventRaised -= HandleEventRaised;
            if (_token.IsCancellationRequested)
            {
                var pf = _context.ProcessFlowInstance;
                pf.CancelElement(_context.CurrentElement);
            }
        }

        private void HandleEventRaised(object sender, DomainEventArgs e)
        {
            var evt = e.DomainEvent as ProcessFlowElementFormConfirmedEvent;
            if (evt == null)
            {
                return;
            }

            if (evt.ElementId != _context.CurrentElement.Id)
            {
                return;
            }

            _context.ProcessFlowInstance.CompletePlanItem(_context.CurrentElement as CMMNPlanItem);
            _context.Complete(_token).Wait();
            _quit = true;
        }
    }
}
