using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithLongProcessTask
{
    public class WaitTaskDelegate : CaseProcessDelegate
    {
        public WaitTaskDelegate(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            await Task.Delay(5 * 1000);
            var result = new CaseProcessResponse();
            await callback(result);
        }
    }
}
