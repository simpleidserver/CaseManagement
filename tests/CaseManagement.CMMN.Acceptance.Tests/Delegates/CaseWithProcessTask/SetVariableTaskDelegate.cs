using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithProcessTask
{
    public class SetVariableTaskDelegate : CaseProcessDelegate
    {
        public override Task<CaseProcessResponse> Handle(CaseProcessParameter parameter)
        {
            var value = parameter.GetStringParameter("processValue");
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "value";
            }
            else
            {
                value += " value";
            }

            var result = new CaseProcessResponse();
            result.AddParameter("processValue", value);
            result.AddParameter("processName", "firstTestProcess");
            return Task.FromResult(result);
        }
    }
}
