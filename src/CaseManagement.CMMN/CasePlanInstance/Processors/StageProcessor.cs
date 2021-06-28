using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class StageProcessor : BaseTaskOrStageProcessor
    {
        private readonly ICMMNProcessorFactory _processorFactory;

        public StageProcessor(ISubscriberRepository subscriberRepository, ICMMNProcessorFactory processorFactory) : base(subscriberRepository)
        {
            _processorFactory = processorFactory;
        }

        public override CasePlanElementInstanceTypes Type => CasePlanElementInstanceTypes.STAGE;

        protected override async Task<bool> ProtectedProcess(CMMNExecutionContext executionContext, CaseEltInstance stageElt, CancellationToken cancellationToken)
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

        private async Task ExecuteNode(CMMNExecutionContext executionContext, CaseEltInstance node, CancellationToken token)
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

        private bool IsElementCompleted(CaseEltInstance planElementInstance)
        {
            if (planElementInstance.IsTaskOrStage() && (planElementInstance.TakeStageState == TaskStageStates.Completed ||
                planElementInstance.TakeStageState == TaskStageStates.Terminated ||
                planElementInstance.TakeStageState == TaskStageStates.Disabled))
            {
                return true;
            }

            if (planElementInstance.IsMilestone() && (planElementInstance.MilestoneState == MilestoneEventStates.Completed ||
                planElementInstance.MilestoneState == MilestoneEventStates.Terminated))
            {
                return true;
            }

            return false;
        }
    }
}
