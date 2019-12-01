using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates
{
    public class CountBikesTaskDelegate : CaseProcessDelegate
    {
        public override Task<CaseProcessResponse> Handle(CaseProcessParameter parameter)
        {
            var result = new CaseProcessResponse();
            result.AddParameter("nbBikes", 21);
            return Task.FromResult(result);
        }
    }
}
