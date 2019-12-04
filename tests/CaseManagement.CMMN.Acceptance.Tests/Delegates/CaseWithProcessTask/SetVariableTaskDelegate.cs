using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithProcessTask
{
    public class SetVariableTaskDelegate : CaseProcessDelegate
    {
        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
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
            return callback(result);
        }
    }
}
