using CaseManagement.CMMN.CaseInstance.Processors.Listeners;
using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : ICMMNPlanItemProcessor
    {
        public abstract CMMNPlanItemDefinitionTypes Type { get; }

        public Task Handle(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var task = new Task(() => HandleTask(parameter, token), token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        protected abstract Task Run(PlanItemProcessorParameter parameter, CancellationToken token);

        private void HandleTask(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            CMMNCriterionListener.Listen(parameter);
            var isManuallyActivated = CMMNManualActivationListener.Listen(parameter);
            /*
            if (!isManuallyActivated)
            {
                pf.StartPlanItem(cmmnPlanItem);
            }

            cmmnPlanItem.StateChanged += HandleSuspend;
            cmmnPlanItem.StateChanged += HandleResume;
            Thread.Sleep(100);
            Run(context, token).Wait();
            */
        }

        private void HandleSuspend(object obj, string state)
        {
            var name = Enum.GetName(typeof(CMMNPlanItemTransitions), CMMNPlanItemTransitions.Suspend);
            if (name != state)
            {
                return;
            }

            // _processTaskTokenSource.Cancel();
        }

        private void HandleResume(object obj, string state)
        {
            var name = Enum.GetName(typeof(CMMNPlanItemTransitions), CMMNPlanItemTransitions.Resume);
            if (name != state)
            {
                return;
            }
            
            // _processTask.Start();
        }
    }
}
