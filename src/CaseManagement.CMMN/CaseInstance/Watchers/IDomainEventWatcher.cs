using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public interface IDomainEventWatcher : IWorkflowSubProcess
    {
        bool Quit { get; set; }
        void AddCallback(EventHandler<DomainEventArgs> callback);
    }
}
