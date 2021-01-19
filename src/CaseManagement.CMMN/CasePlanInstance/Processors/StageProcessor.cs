using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Collections.Generic;
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
            var executionBranch = ExecutionBranch.Build(stageElt.Children);
            await ExecuteBranch(executionContext, executionBranch, cancellationToken);
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

        private async Task ExecuteBranch(CMMNExecutionContext executionContext, BaseExecutionBranch<BaseCasePlanItemInstance> branch, CancellationToken cancellationToken)
        {
            var taskLst = new List<Task>();
            foreach (var node in branch.Nodes)
            {
                taskLst.Add(HandleCasePlan(executionContext, node, cancellationToken));
            }

            await Task.WhenAll(taskLst);
            if (branch.NextBranch != null)
            {
                await ExecuteBranch(executionContext, branch.NextBranch, cancellationToken);
            }
        }

        private async Task HandleCasePlan(CMMNExecutionContext executionContext, BaseCasePlanItemInstance casePlanElementInstance, CancellationToken token)
        {
            await _processorFactory.Execute(executionContext, casePlanElementInstance, token);
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
