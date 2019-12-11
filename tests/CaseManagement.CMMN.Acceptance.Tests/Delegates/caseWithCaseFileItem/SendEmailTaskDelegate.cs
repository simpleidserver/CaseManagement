using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates.caseWithCaseFileItem
{
    public class SendEmailTaskDelegate : CaseProcessDelegate
    {
        public SendEmailTaskDelegate(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var factory = (ICaseFileItemRepositoryFactory)ServiceProvider.GetService(typeof(ICaseFileItemRepositoryFactory));
            var caseFileItem = JsonConvert.DeserializeObject<CMMNCaseFileItem>(parameter.GetStringParameter("caseFileItem"));
            var repository = factory.Get(caseFileItem);
            var folder = repository.Get(caseFileItem);
            var result = new CaseProcessResponse();
            foreach(var child in folder.GetChildren())
            {
                result.AddParameter("fileContent", child.ReadContent());
            }

            return callback(result);
        }
    }
}
