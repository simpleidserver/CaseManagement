using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates
{
    public class CountCarsTaskDelegate : CaseProcessDelegate
    {
        public CountCarsTaskDelegate(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var result = new CaseProcessResponse();
            result.AddParameter("nbCars", 51);
            return callback(result);
        }
    }
}
