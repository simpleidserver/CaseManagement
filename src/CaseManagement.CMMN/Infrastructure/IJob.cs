using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure
{
    public interface IJob
    {
        Task Start();
        Task Stop();
    }
}