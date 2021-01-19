using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN
{
    public interface ICaseJobServer
    {
        void Start();
        void Stop();
        Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken token);
        Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId, Dictionary<string, string> parameters, CancellationToken token);
        Task EnqueueCasePlanInstance(string casePlanInstanceId, CancellationToken token);
    }
}
