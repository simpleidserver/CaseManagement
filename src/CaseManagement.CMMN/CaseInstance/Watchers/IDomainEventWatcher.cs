using System;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public interface IDomainEventWatcher
    {
        bool Quit { get; set; }
        // void AddCallback(EventHandler<DomainEventArgs> callback);
    }
}
