using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CasePlanInstance;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
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

        protected override async Task ProtectedExecute(ExecutionContext executionContext, StageElementInstance stageElt, CancellationToken cancellationToken)
        {
            var executionBranch = ExecutionBranch.Build(stageElt.Children);
            await ExecuteBranch(executionContext, executionBranch, cancellationToken);
            if (stageElt.Children.All(_ => IsElementCompleted(_)))
            {
                executionContext.CasePlanInstance.MakeTransition(stageElt, CMMNTransitions.Complete);
            }
        }

        private async Task ExecuteBranch(ExecutionContext executionContext, ExecutionBranch branch, CancellationToken cancellationToken)
        {
            var taskLst = new List<Task>();
            foreach(var node in branch.Nodes)
            {
                taskLst.Add(HandleCasePlan(executionContext, node, cancellationToken));
            }

            await Task.WhenAll(taskLst);
            if (branch.NextBranch != null)
            {
                await ExecuteBranch(executionContext, branch.NextBranch, cancellationToken);
            }
        }

        private Task HandleCasePlan(ExecutionContext executionContext, CasePlanElementInstance casePlanElementInstance, CancellationToken token)
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
            if (stageOrTask != null && (stageOrTask.State == TaskStageStates.Completed || stageOrTask.State == TaskStageStates.Terminated || stageOrTask.State != TaskStageStates.Terminated))
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