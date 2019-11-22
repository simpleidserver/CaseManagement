using System;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;

namespace CaseManagement.CMMN.ProcessInstance.Processors
{
    public class CMMNPlanItemProcessor : IProcessFlowElementProcessor
    {
        public virtual Type ProcessFlowElementType => typeof(CMMNPlanItem);

        public virtual Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            var processTask = (CMMNPlanItem)pfe;
            processTask.Start();
            if (!CheckCanBeExecuted(processTask, context))
            {
                return Task.FromResult(0);
            }

            pfe.Finish();
            return Task.FromResult(0);

        }

        protected bool CheckCanBeExecuted(CMMNPlanItem pfe, ProcessFlowInstanceExecutionContext context)
        {
            var planItem = (CMMNPlanItem)pfe;
            foreach(var se in planItem.SEntries)
            {
                if (CheckSEntry(se, context))
                {
                    return true;
                }
            }

            return !planItem.SEntries.Any();
        }

        private bool CheckSEntry(CMMNSEntry sEntry, ProcessFlowInstanceExecutionContext context)
        {
            foreach (var onPart in sEntry.OnParts)
            {
                if (onPart is CMMNPlanItemOnPart)
                {
                    if (!string.IsNullOrWhiteSpace(onPart.SourceRef))
                    {
                        var elt = context.GetPlanItem(onPart.SourceRef);
                        if (elt == null || elt.Transition != onPart.StandardEvent)
                        {
                            return false;
                        }
                    }
                }
            }

            if (sEntry.IfPart != null)
            {
                return ExpressionParser.IsValid(sEntry.IfPart.Condition, context);
            }

            return true;
        }
    }
}
