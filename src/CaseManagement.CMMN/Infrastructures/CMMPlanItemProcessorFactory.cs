using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Infrastructures
{
    public class CMMPlanItemProcessorFactory : IProcessFlowElementProcessorFactory
    {
        private readonly IEnumerable<IProcessFlowElementProcessor> _processFlowElementProcessors;

        public CMMPlanItemProcessorFactory(IEnumerable<IProcessFlowElementProcessor> processFlowElementProcessors)
        {
            _processFlowElementProcessors = processFlowElementProcessors;
        }

        public IProcessFlowElementProcessor Build(ProcessFlowInstanceElement elt)
        {
            var cmmnPlanItem = elt as CMMNPlanItem;
            var processor = _processFlowElementProcessors.First(p => p.ProcessFlowElementType == Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), cmmnPlanItem.PlanItemDefinitionType).ToLowerInvariant());
            return processor;
        }
    }
}
