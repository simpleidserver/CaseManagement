using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public interface ICaseProcessHandler
    {
        string ImplementationType { get; }
        Task<CaseProcessResponse> Handle(ProcessAggregate process, CaseProcessParameter parameter);
    }
}
