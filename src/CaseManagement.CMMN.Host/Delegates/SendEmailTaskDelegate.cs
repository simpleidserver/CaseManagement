using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Host.Delegates
{
    public class SendEmailTaskDelegate : CaseProcessDelegate
    {
        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            return callback(new CaseProcessResponse());
        }
    }
}
