using CaseManagement.Workflow.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public class RunningTask
    {
        public RunningTask(string processId, Task task, ProcessFlowInstance processFlowInstance, CancellationTokenSource cancellationTokenSource)
        {
            ProcessId = processId;
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
            ProcessFlowInstance = processFlowInstance;
        }

        public string ProcessId { get; set; }
        public Task Task { get; set; }
        public ProcessFlowInstance ProcessFlowInstance { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
