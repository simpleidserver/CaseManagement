using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface IJob
    {
        Task Start();
        Task Stop();
    }
}
