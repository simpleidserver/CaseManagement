using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public CMMNTaskProcessor(ICommitAggregateHelper commitAggregateHelper) : base(commitAggregateHelper)
        {
        }

        public override CaseElementTypes Type => CaseElementTypes.Task;

        protected override Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            parameter.CaseInstance.MakeTransitionComplete(parameter.CaseElementInstance.Id);
            return Task.CompletedTask;
        }

        protected override void Unsubscribe() { }
    }
}
