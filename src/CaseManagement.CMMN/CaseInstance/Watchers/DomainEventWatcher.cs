using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public class DomainEventWatcher : IDomainEventWatcher
    {
        private List<EventHandler<DomainEventArgs>> _callbackLst;
        private CancellationToken _token;
        private WorkflowHandlerContext _context;

        public DomainEventWatcher()
        {
            _callbackLst = new List<EventHandler<DomainEventArgs>>();
        }

        public Task Task { get; private set; }
        public bool Quit { get; set; }

        public void AddCallback(EventHandler<DomainEventArgs> callback)
        {
            lock(_callbackLst)
            {
                _callbackLst.Add(callback);
            }
        }

        public Task Start(WorkflowHandlerContext context, CancellationToken token)
        {
            Quit = false;
            _token = token;
            _context = context;
            Task = new Task(Handle, token, TaskCreationOptions.LongRunning);
            Task.Start();
            return Task.CompletedTask;
        }

        private void Handle()
        {
            foreach(var callback in _callbackLst)
            {
                _context.ProcessFlowInstance.EventRaised += callback;
            }

            while (!_token.IsCancellationRequested)
            {
                Task.Delay(10).Wait();
                if (Quit)
                {
                    return;
                }
            }

            foreach (var callback in _callbackLst)
            {
                _context.ProcessFlowInstance.EventRaised -= callback;
            }
            if (_token.IsCancellationRequested)
            {
                var pf = _context.ProcessFlowInstance;
                pf.CancelElement(_context.CurrentElement);
            }
        }
    }
}
