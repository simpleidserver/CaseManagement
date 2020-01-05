using System;
using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates
{
    public class LongTask : CaseProcessDelegate
    {
        public LongTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            Thread.Sleep(1 * 1000);
            token.ThrowIfCancellationRequested();
            var result = new CaseProcessResponse();
            return callback(result);
        }
    }
}
