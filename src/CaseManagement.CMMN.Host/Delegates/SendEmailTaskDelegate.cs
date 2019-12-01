using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Host.Delegates
{
    public class SendEmailTaskDelegate : CaseProcessDelegate
    {
        public override Task<CaseProcessResponse> Handle(CaseProcessParameter parameter)
        {
            return Task.FromResult(new CaseProcessResponse());
        }
    }
}
