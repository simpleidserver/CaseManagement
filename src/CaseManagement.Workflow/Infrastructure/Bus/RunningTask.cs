using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public class RunningTask
    {
        public RunningTask(string id, Task task, CancellationTokenSource cancellationTokenSource)
        {
            Id = id;
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
        }

        public string Id { get; set; }
        public Task Task { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
