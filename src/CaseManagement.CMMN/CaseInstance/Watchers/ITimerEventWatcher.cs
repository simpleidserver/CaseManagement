using System;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public interface ITimerEventWatcher
    {
        void ScheduleJob(DateTime dateTime, string processId);
    }
}
