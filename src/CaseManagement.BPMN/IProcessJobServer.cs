using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN
{
    public interface IProcessJobServer
    {
        void Start();
        void Stop();
        Task RegisterProcessInstance(ProcessInstanceAggregate processInstance, CancellationToken token);
        Task EnqueueProcessInstance(string processInstanceId, CancellationToken token);
    }
}
