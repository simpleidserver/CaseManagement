using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.CaseWithRepetitionRule
{
    public class CountClientsTask : CaseProcessDelegate
    {
        public CountClientsTask(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var value = parameter.GetIntParameter("nbClients");
            if (value == default(int))
            {
                value = 1;
            }
            else
            {
                value++;
            }

            var result = new CaseProcessResponse();
            result.AddParameter("nbClients", value);
            return callback(result);
        }
    }
}
