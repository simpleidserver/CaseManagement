using CaseManagement.CMMN.CaseInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNCaseFileItemProcessor : IProcessor
    {
        private readonly IEnumerable<ICaseFileItemListener> _caseFileItemListeners;

        public CMMNCaseFileItemProcessor(IEnumerable<ICaseFileItemListener> caseFileItemListeners)
        {
            _caseFileItemListeners = caseFileItemListeners;
        }

        public CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.CaseFileItem;

        public async Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var caseFile = parameter.WorkflowInstance.GetWorkflowElementDefinition(parameter.WorkflowElementInstance.Id, parameter.WorkflowDefinition) as CMMNCaseFileItemDefinition;
            var caseFileItemListener = _caseFileItemListeners.First(c => c.CaseFileItemType == caseFile.Definition);
            await caseFileItemListener.Start(parameter, token);
            return parameter;
        }
    }
}