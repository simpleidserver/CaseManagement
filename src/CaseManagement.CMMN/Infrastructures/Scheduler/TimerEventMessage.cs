using CaseManagement.Workflow.Infrastructure.Scheduler;

namespace CaseManagement.CMMN.Infrastructures.Scheduler
{
    public class TimerEventMessage : JobMessage
    {
        public string ProcessId { get; set; }
        public string ElementId { get; set; }
    }
}
