using System;
using System.Threading;
using System.Threading.Tasks;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithRepetitionRule
{
    public class SendEmailToClientTask : CaseProcessDelegate
    {
        public SendEmailToClientTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var result = new CaseProcessResponse();
            return callback(result);
        }
    }
}
