using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNCaseFileItemProcessor : IProcessFlowElementProcessor
    {
        private readonly ICaseFileItemRepositoryFactory _caseFileItemRepositoryFactory;

        public CMMNCaseFileItemProcessor(ICaseFileItemRepositoryFactory caseFileItemRepositoryFactory)
        {
            _caseFileItemRepositoryFactory = caseFileItemRepositoryFactory;
        }

        public string ProcessFlowElementType => CMMNCaseFileItem.ELEMENT_TYPE;

        public async Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            context.Start(token);
            var caseFileItem = context.GetCMMNCaseFileItem();
            var repository = _caseFileItemRepositoryFactory.Get(caseFileItem);
            await context.StartSubProcess(repository, token);
            await context.Complete(token);
        }
    }
}
