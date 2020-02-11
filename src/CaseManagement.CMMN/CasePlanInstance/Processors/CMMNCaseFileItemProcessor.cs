using CaseManagement.CMMN.CasePlanInstance.Processors.CaseFileItem;
using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNCaseFileItemProcessor : IProcessor
    {
        private readonly IEnumerable<ICaseFileItemListener> _caseFileItemListeners;

        public CMMNCaseFileItemProcessor(IEnumerable<ICaseFileItemListener> caseFileItemListeners)
        {
            _caseFileItemListeners = caseFileItemListeners;
        }

        public CaseElementTypes Type => CaseElementTypes.CaseFileItem;

        public async Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token)
        {
            var caseFile = parameter.CaseInstance.GetWorkflowElementDefinition(parameter.CaseElementInstance.Id, parameter.CaseDefinition) as CaseFileItemDefinition;
            var caseFileItemListener = _caseFileItemListeners.First(c => c.CaseFileItemType == caseFile.Definition);
            await caseFileItemListener.Start(parameter, token);
            return parameter;
        }
    }
}