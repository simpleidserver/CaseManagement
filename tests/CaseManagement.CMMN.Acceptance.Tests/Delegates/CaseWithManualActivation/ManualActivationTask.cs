using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithManualActivation
{
    public class ManualActivationTask : CaseProcessDelegate
    {
        public ManualActivationTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var result = new CaseProcessResponse();
            result.AddParameter("variable", 1);
            return callback(result);
        }
    }
}
