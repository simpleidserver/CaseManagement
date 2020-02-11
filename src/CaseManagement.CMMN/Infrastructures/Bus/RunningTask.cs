using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public class RunningTask
    {
        public RunningTask(string processId, Task task, BaseAggregate aggregagate)
        {
            ProcessId = processId;
            Task = task;
            Aggregate = aggregagate;
        }

        public string ProcessId { get; set; }
        public Task Task { get; set; }
        public BaseAggregate Aggregate { get; set; }
    }
}
