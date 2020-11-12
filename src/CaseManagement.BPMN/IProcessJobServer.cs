using CaseManagement.BPMN.Domains;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN
{
    public interface IProcessJobServer
    {
        void Start();
        void Stop();
        Task RegisterProcessInstance(ProcessInstanceAggregate processInstance, CancellationToken token);
        Task EnqueueProcessInstance(string processInstanceId, bool isNewInstance, CancellationToken token);
        Task EnqueueStateTransition(string processInstanceId, string flowNodeInstanceId, string state, JObject jObj, CancellationToken token);
        Task EnqueueMessage(string processInstanceId, string messageName, object content, CancellationToken token);
    }
}
