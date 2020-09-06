using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN
{
    public interface ICaseJobServer
    {
        void Start();
        void Stop();
        Task RegisterCasePlanInstance(CasePlanInstanceAggregate casePlanInstance, CancellationToken cancellationToken);
        Task PublishExternalEvt(string evt, string casePlanInstanceId, string casePlanElementInstanceId);
        Task EnqueueCasePlanInstance(string id);
    }
}
