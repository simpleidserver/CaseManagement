using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNStageProcessor : ICMMNPlanItemProcessor
    {
        public CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.Stage;

        public Task<PlanItemProcessorParameter> Handle(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task<PlanItemProcessorParameter>(() => HandleTask(parameter, cancellationTokenSource), TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private PlanItemProcessorParameter HandleTask(PlanItemProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            CMMNCriterionListener.Listen(parameter);
            var isManuallyActivated = CMMNManualActivationListener.Listen(parameter);
            if (!isManuallyActivated)
            {
                parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Start);
            }

            
            var cmmnStageDefinition = parameter.WorkflowInstance.GetWorkflowElementDefinition(parameter.WorkflowElementInstance.Id, parameter.WorkflowDefinition) as CMMNStageDefinition;
            foreach (var elt in cmmnStageDefinition.Elements)
            {
                parameter.WorkflowInstance.CreateWorkflowElementInstance(elt, parameter.WorkflowElementInstance.Id);
            }

            var children = cmmnStageDefinition.Elements.Select(e => e.Id);
            bool isSuspend = false;
            bool continueExecution = true;
            var parentTerminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
            {
                tokenSource.Cancel();
                var workflowElementInstances = parameter.WorkflowInstance.GetWorkflowElementInstancesByParentId(parameter.WorkflowElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.WorkflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentTerminate);
                }
            });
            var parentSuspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
            {
                isSuspend = true;
                var workflowElementInstances = parameter.WorkflowInstance.GetWorkflowElementInstancesByParentId(parameter.WorkflowElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.WorkflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentSuspend);
                }
            });
            var parentResumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
            {
                isSuspend = false;
                var workflowElementInstances = parameter.WorkflowInstance.GetWorkflowElementInstancesByParentId(parameter.WorkflowElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.WorkflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentResume);
                }
            });
            var suspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
            {
                isSuspend = true;
                var workflowElementInstances = parameter.WorkflowInstance.GetWorkflowElementInstancesByParentId(parameter.WorkflowElementInstance.Id);
                foreach(var workflowElementInstance in workflowElementInstances)
                {
                    parameter.WorkflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentSuspend);
                }
            });
            var resumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
            {
                isSuspend = false;
                var workflowElementInstances = parameter.WorkflowInstance.GetWorkflowElementInstancesByParentId(parameter.WorkflowElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.WorkflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentResume);
                }
            });
            var terminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
            {
                tokenSource.Cancel();
                var workflowElementInstances = parameter.WorkflowInstance.GetWorkflowElementInstancesByParentId(parameter.WorkflowElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.WorkflowInstance.MakeTransition(workflowElementInstance.Id, CMMNTransitions.ParentTerminate);
                }
            });
            while (continueExecution)
            {
                Thread.Sleep(100);
                if (isSuspend)
                {
                    continue;
                }

                try
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    if (children.All(c => parameter.WorkflowInstance.IsWorkflowElementDefinitionFinished(c)))
                    {
                        continueExecution = false;
                        parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Complete);
                    }
                }
                catch(OperationCanceledException)
                {
                    continueExecution = false;
                }
            }

            parentTerminateEvtListener.Unsubscribe();
            parentSuspendEvtListener.Unsubscribe();
            parentResumeEvtListener.Unsubscribe();
            suspendEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
            return parameter;
        }
    }
}
