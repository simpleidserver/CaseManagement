using System;
using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;

namespace CaseManagement.CMMN.Tests.Delegates
{
    public class FailedTask : CaseProcessDelegate
    {
        public FailedTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            throw new Exception("message");
        }
    }
}
