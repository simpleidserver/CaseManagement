using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IFormQueryRepository
    {
        Task<Form> FindFormById(string id);
    }
}
