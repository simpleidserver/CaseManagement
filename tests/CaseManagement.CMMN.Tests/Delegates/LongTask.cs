using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Tests.Delegates
{
    public class LongTask : CaseProcessDelegate
    {
        private const double NUMBER_MS = 2000;

        public LongTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var continueExecution = true;
            var currentDateTime = DateTime.UtcNow;
            while(continueExecution)
            {
                Thread.Sleep(10);
                token.ThrowIfCancellationRequested();
                double diff = (DateTime.UtcNow - currentDateTime).TotalMilliseconds;
                if (diff >= NUMBER_MS)
                {
                    continueExecution = false;
                }
            }

            var result = new CaseProcessResponse();
            return callback(result);
        }
    }
}
