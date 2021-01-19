using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class StageProcessor : BaseTaskOrStageProcessor<StageElementInstance>
    {
        private readonly IProcessorFactory _processorFactory;

        public StageProcessor(ISubscriberRepository subscriberRepository, IProcessorFactory processorFactory) : base(subscriberRepository)
        {
            _processorFactory = processorFactory;
        }

        protected override async Task<bool> ProtectedProcess(CMMNExecutionContext executionContext, StageElementInstance stageElt, CancellationToken cancellationToken)
        {
            var rootNodes = stageElt.Children.Where(_ => _.IsLeaf()).ToList();
            foreach(var rootNode in rootNodes)
            {
                await ExecuteNode(executionContext, rootNode, cancellationToken);
            }

            if (stageElt.Children.All(_ => IsElementCompleted(_)))
            {
                executionContext.Instance.MakeTransition(stageElt, CMMNTransitions.Complete);
                return true;
            }

            if (executionContext.Instance.IsExitCriteriaSatisfied(stageElt).IsSatisfied)
            {
                executionContext.Instance.MakeTransition(stageElt, CMMNTransitions.Terminate);
                return true;
            }

            return false;
        }

        private async Task ExecuteNode(CMMNExecutionContext executionContext, BaseCasePlanItemInstance node, CancellationToken token)
        {
            await _processorFactory.Execute(executionContext, node, token);
            var domainEvts = executionContext.Instance.DomainEvents.Where((evt) =>
            {
                var r = evt as CaseElementTransitionRaisedEvent;
                if (r == null)
                {
                    return false;
                }

                return r.ElementId == node.Id;
            }).Cast<CaseElementTransitionRaisedEvent>()
            .Select(_ => new IncomingTransition(_.Transition, _.IncomingTokens)).ToList();
            var nextNodes = executionContext.Instance.GetNextCasePlanItems(node);
            foreach(var nextNode in nextNodes)
            {
                executionContext.Instance.ConsumeTransitionEvts(nextNode, node.Id, domainEvts);
                await ExecuteNode(executionContext, nextNode, token);
            }
        }

        private bool IsElementCompleted(BaseCasePlanItemInstance planElementInstance)
        {
            var stageOrTask = planElementInstance as BaseTaskOrStageElementInstance;
            var milestone = planElementInstance as MilestoneElementInstance;
            if (stageOrTask != null && (stageOrTask.State == TaskStageStates.Completed ||
                stageOrTask.State == TaskStageStates.Terminated ||
                stageOrTask.State == TaskStageStates.Disabled))
            {
                return true;
            }

            if (milestone != null && (milestone.State == MilestoneEventStates.Completed || milestone.State == MilestoneEventStates.Terminated))
            {
                return true;
            }

            return false;
        }
    }
}
