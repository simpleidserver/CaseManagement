using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CaseManagement.CMMN.CaseInstance.Processors.Listeners.CMMNCriterionListener;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNStageProcessor : IProcessor
    {
        public CaseElementTypes Type => CaseElementTypes.Stage;

        public Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task<ProcessorParameter>(() => HandleTask(parameter, cancellationTokenSource), TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private ProcessorParameter HandleTask(ProcessorParameter parameter, CancellationTokenSource tokenSource)
        {
            CMMNCriterionListener.ListenEntryCriterias(parameter);
            var isManuallyActivated = CMMNManualActivationListener.Listen(parameter);
            if (!isManuallyActivated)
            {
                parameter.CaseInstance.MakeTransitionStart(parameter.CaseElementInstance.Id);
            }
            
            var cmmnStageDefinition = (parameter.CaseInstance.GetWorkflowElementDefinition(parameter.CaseElementInstance.Id, parameter.CaseDefinition) as PlanItemDefinition).Stage;
            foreach (var elt in cmmnStageDefinition.Elements)
            {
                parameter.CaseInstance.CreateWorkflowElementInstance(elt, parameter.CaseElementInstance.Id);
            }

            var children = cmmnStageDefinition.Elements.Select(e => e.Id);
            bool isSuspend = false;
            bool continueExecution = true;
            var parentExitEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentExit, () =>
            {
                tokenSource.Cancel();
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentExit(workflowElementInstance.Id);
                }
            });
            var parentTerminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentTerminate, () =>
            {
                tokenSource.Cancel();
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentTerminate(workflowElementInstance.Id);
                }
            });
            var parentSuspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentSuspend, () =>
            {
                isSuspend = true;
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentSuspend(workflowElementInstance.Id);
                }
            });
            var parentResumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.ParentResume, () =>
            {
                isSuspend = false;
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentResume(workflowElementInstance.Id);
                }
            });
            var exitEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Exit, () =>
            {
                tokenSource.Cancel();
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentExit(workflowElementInstance.Id);
                }
            });
            var reactivateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Reactivate, () =>
            {
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionReactivate(workflowElementInstance.Id);
                }

                isSuspend = false;
            });
            var suspendEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Suspend, () =>
            {
                isSuspend = true;
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach(var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentSuspend(workflowElementInstance.Id);
                }
            });
            var resumeEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Resume, () =>
            {
                isSuspend = false;
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentResume(workflowElementInstance.Id);
                }
            });
            var terminateEvtListener = CMMNPlanItemTransitionListener.Listen(parameter, CMMNTransitions.Terminate, () =>
            {
                tokenSource.Cancel();
                var workflowElementInstances = parameter.CaseInstance.GetWorkflowElementInstancesByParentId(parameter.CaseElementInstance.Id);
                foreach (var workflowElementInstance in workflowElementInstances)
                {
                    parameter.CaseInstance.MakeTransitionParentTerminate(workflowElementInstance.Id);
                }
            });
            KeyValuePair<Task, CriterionListener>? kvp = null;
            try
            {
                kvp = CMMNCriterionListener.ListenExitCriterias(parameter);
                if (kvp != null)
                {
                    kvp.Value.Key.ContinueWith((r) =>
                    {
                        r.Wait();
                        parameter.CaseInstance.MakeTransitionExit(parameter.CaseElementInstance.Id);
                    });
                }
            }
            catch (TerminateCaseInstanceElementException)
            {
                parameter.CaseInstance.MakeTransitionExit(parameter.CaseElementInstance.Id);
            }

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
                    if (children.Any(c => parameter.CaseInstance.IsWorkflowElementDefinitionFailed(c) && parameter.CaseDefinition.GetElement(c).Type != CaseElementTypes.Stage))
                    {
                        isSuspend = true;
                        parameter.CaseInstance.MakeTransitionFault(parameter.CaseElementInstance.Id);
                    }
                    else if (children.All(c => parameter.CaseInstance.IsWorkflowElementDefinitionFinished(c)))
                    {
                        continueExecution = false;
                        parameter.CaseInstance.MakeTransitionComplete(parameter.CaseElementInstance.Id);
                    }
                }
                catch(OperationCanceledException)
                {
                    continueExecution = false;
                }
            }
            
            parentExitEvtListener.Unsubscribe();
            parentTerminateEvtListener.Unsubscribe();
            parentSuspendEvtListener.Unsubscribe();
            parentResumeEvtListener.Unsubscribe();
            exitEvtListener.Unsubscribe();
            reactivateEvtListener.Unsubscribe();
            suspendEvtListener.Unsubscribe();
            resumeEvtListener.Unsubscribe();
            terminateEvtListener.Unsubscribe();
            if (kvp != null)
            {
                if (kvp.Value.Key.IsCanceled || kvp.Value.Key.IsCompleted || kvp.Value.Key.IsFaulted)
                {
                    kvp.Value.Key.Dispose();
                }

                kvp.Value.Value.Unsubscribe();
            }

            return parameter;
        }
    }
}
