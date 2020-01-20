using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public class RunningTaskPool : IRunningTaskPool
    {
        private readonly List<RunningTask> _runningTasks;

        public IEnumerable<Task> Tasks => _runningTasks.Select(r => r.Task);

        public RunningTaskPool()
        {
            _runningTasks = new List<RunningTask>();
        }

        public int NbTasks()
        {
            return _runningTasks.Count();
        }

        public void RemoveTask(string processId)
        {
            lock(_runningTasks)
            {
                _runningTasks.Remove(_runningTasks.First(a => a.ProcessId == processId));
            }
        }

        public void AddTask(RunningTask task)
        {
            lock (_runningTasks)
            {
                _runningTasks.Add(task);
            }
        }

        public RunningTask Get(string processId)
        {
            return _runningTasks.FirstOrDefault(r => r.ProcessId == processId);
        }
    }
}
