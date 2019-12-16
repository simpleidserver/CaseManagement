using CaseManagement.Workflow.Engine;
using System;

namespace CaseManagement.CMMN.CaseInstance.Watchers
{
    public interface ITimerEventWatcher : IWorkflowSubProcess
    {
        void ScheduleJob(DateTime dateTime, string processId);
    }
}
