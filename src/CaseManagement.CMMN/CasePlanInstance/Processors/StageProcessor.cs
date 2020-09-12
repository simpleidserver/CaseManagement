using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
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

        protected override async Task ProtectedExecute(CMMNExecutionContext executionContext, StageElementInstance stageElt, CancellationToken cancellationToken)
        {
            var executionBranch = ExecutionBranch.Build(stageElt.Children);
            await ExecuteBranch(executionContext, executionBranch, cancellationToken);
            if (stageElt.Children.All(_ => IsElementCompleted(_)))
            {
                executionContext.CasePlanInstance.MakeTransition(stageElt, CMMNTransitions.Complete);
                return;
            }

            if (executionContext.CasePlanInstance.IsExitCriteriaSatisfied(stageElt))
            {
                executionContext.CasePlanInstance.MakeTransition(stageElt, CMMNTransitions.Terminate);
            }
        }

        private async Task ExecuteBranch(CMMNExecutionContext executionContext, ExecutionBranch branch, CancellationToken cancellationToken)
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

        private Task HandleCasePlan(CMMNExecutionContext executionContext, CasePlanElementInstance casePlanElementInstance, CancellationToken token)
        {
            if (!executionContext.CasePlanInstance.IsEntryCriteriaSatisfied(casePlanElementInstance.Id))
            {
                return Task.CompletedTask;
            }

            return _processorFactory.Execute(executionContext, casePlanElementInstance, casePlanElementInstance.GetType(), token);
        }

        private bool IsElementCompleted(CasePlanElementInstance planElementInstance)
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
