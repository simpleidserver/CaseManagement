using System.Threading.Tasks;

namespace CaseManagement.Common.Jobs
{
    public interface IJob
    {
        Task Start();
        Task Stop();
    }
}