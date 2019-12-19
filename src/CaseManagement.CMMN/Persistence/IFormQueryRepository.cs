using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IFormQueryRepository
    {
        Task<FormAggregate> FindFormById(string id);
    }
}
