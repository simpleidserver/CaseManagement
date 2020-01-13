using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public override CaseElementTypes Type => CaseElementTypes.Task;

        protected override Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            parameter.CaseInstance.MakeTransitionComplete(parameter.CaseElementInstance.Id);
            return Task.CompletedTask;
        }

        protected override void Unsubscribe() { }
    }
}
