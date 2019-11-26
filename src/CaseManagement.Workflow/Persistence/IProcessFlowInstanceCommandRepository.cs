using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IProcessFlowInstanceCommandRepository
    {
        void Add(ProcessFlowInstance processFlowInstance);
        void Update(ProcessFlowInstance processFlowInstance);
        Task<int> SaveChanges();
    }
}
