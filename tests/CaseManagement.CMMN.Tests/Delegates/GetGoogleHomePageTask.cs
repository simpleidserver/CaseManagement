using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Tests.Delegates
{
    public class GetGoogleHomePageTask : CaseProcessDelegate
    {
        public GetGoogleHomePageTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            using (var httpClient = new HttpClient())
            {
                var httpResult = await httpClient.GetAsync("http://google.com", token);
                var txt = await httpResult.Content.ReadAsStringAsync();
                var result = new CaseProcessResponse();
                result.AddParameter("html", txt);
                await callback(result);
            }
        }
    }
}
