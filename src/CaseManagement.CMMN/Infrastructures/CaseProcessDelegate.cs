using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public abstract class CaseProcessDelegate
    {
        public abstract Task<CaseProcessResponse> Handle(CaseProcessParameter parameter);
    }
}
