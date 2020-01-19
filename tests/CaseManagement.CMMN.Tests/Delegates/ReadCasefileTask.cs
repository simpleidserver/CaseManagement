using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Tests.Delegates
{
    public class ReadCasefileTask : CaseProcessDelegate
    {
        public ReadCasefileTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var caseFileItemRepository = (ICaseFileItemRepository)ServiceProvider.GetService(typeof(ICaseFileItemRepository));
            var caseInstanceId = parameter.GetCaseInstanceId();
            var caseFileItem = await caseFileItemRepository.FindByCaseInstance(caseInstanceId);
            var child = caseFileItem.First().GetChildren().First();
            var content = child.ReadContent();
            var result = new CaseProcessResponse();
            result.AddParameter("contentFile", content);
            await callback(result);
        }
    }
}
