using CaseManagement.CMMN.CaseInstance.Watchers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNHumanTaskProcessor : BaseCMMNTaskProcessor
    {
        private List<string> _completeElements;

        public CMMNHumanTaskProcessor(IDomainEventWatcher domainEventWatcher) : base(domainEventWatcher)
        {
            _completeElements = new List<string>();
        }

        public override string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.HumanTask).ToLowerInvariant();

        public override async Task Run(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var humanTask = context.GetCMMNHumanTask();
            var formInstance = pf.GetFormInstance(context.CurrentElement.Id);
            if (formInstance == null && !string.IsNullOrWhiteSpace(humanTask.FormId))
            {
                pf.CreateForm(cmmnPlanItem.Id, humanTask.FormId, humanTask.PerformerRef);
            }

            else if (formInstance != null && formInstance.Status == FormInstanceStatus.Complete)
            {
                pf.CompletePlanItem(cmmnPlanItem);
                await context.ExecuteNext(token);
                return;
            }

            if (humanTask.IsBlocking)
            {
                pf.BlockElement(cmmnPlanItem);
                DomainEventWatcher.AddCallback(async (o, e) =>
                {
                    var evt = e.DomainEvent as ProcessFlowElementFormConfirmedEvent;
                    if (evt == null)
                    {
                        return;
                    }

                    if (evt.ElementId != context.CurrentElement.Id)
                    {
                        return;
                    }


                    pf.UnblockElement(cmmnPlanItem);
                    await context.ExecuteNext(token);
                    context.ProcessFlowInstance.CompletePlanItem(context.CurrentElement as CMMNPlanItem);
                    if (_completeElements.Contains($"{context.ProcessFlowInstance.Id}_{context.CurrentElement.Id}"))
                    {
                        context.Complete();
                    }

                    DomainEventWatcher.Quit = true;
                });
                return;
            }

            pf.CompletePlanItem(cmmnPlanItem);
            await context.ExecuteNext(token);
            return;
        }

        protected override void Complete(WorkflowHandlerContext context)
        {
            lock (_completeElements)
            {
                _completeElements.Add($"{context.ProcessFlowInstance.Id}_{context.CurrentElement.Id}");
            }

            DomainEventWatcher.Quit = true;
        }
    }
}
