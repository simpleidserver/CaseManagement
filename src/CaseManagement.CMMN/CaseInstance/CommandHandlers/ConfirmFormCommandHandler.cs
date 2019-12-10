using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Process.Exceptions;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Persistence;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class ConfirmFormCommandHandler : IConfirmFormCommandHandler
    {
        private readonly IQueueProvider _queueProvider;
        private readonly IFormQueryRepository _formQueryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;

        public ConfirmFormCommandHandler(IQueueProvider queueProvider, IFormQueryRepository formQueryRepository, IEventStoreRepository eventStoreRepository)
        {
            _queueProvider = queueProvider;
            _formQueryRepository = formQueryRepository;
            _eventStoreRepository = eventStoreRepository;
        }
        
        public async Task<bool> Handle(ConfirmFormCommand confirmFormCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<CMMNProcessFlowInstance>(confirmFormCommand.CaseInstanceId, CMMNProcessFlowInstance.GetCMMNStreamName(confirmFormCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(confirmFormCommand.CaseInstanceId);
            }

            var flowInstanceElt = caseInstance.Elements.FirstOrDefault(e => e.Id == confirmFormCommand.CaseElementInstanceId) as CMMNPlanItem;
            if (flowInstanceElt == null)
            {
                throw new UnknownCaseInstanceElementException(caseInstance.Id, confirmFormCommand.CaseElementInstanceId);
            }
            
            if (flowInstanceElt.PlanItemDefinitionType != CMMNPlanItemDefinitionTypes.HumanTask)
            {
                throw new NotSupportedTaskException("Task must be a HumanTask");
            }

            var humanTask = flowInstanceElt.PlanItemDefinitionHumanTask;
            var form = await _formQueryRepository.FindFormById(humanTask.FormId);
            if (form == null)
            {
                throw new UnknownFormException(humanTask.FormId);
            }

            var formValues = CheckConfirmForm(form, confirmFormCommand.Content);
            await _queueProvider.QueueConfirmForm(caseInstance.Id, confirmFormCommand.CaseElementInstanceId, formValues);
            return true;
        }

        private static Dictionary<string, string> CheckConfirmForm(FormAggregate form, JObject content)
        {
            var result = new Dictionary<string, string>();
            var errors = new Dictionary<string, string>();
            foreach (var elt in form.Elements)
            {
                string value = string.Empty;
                if (elt.IsRequired && (!content.ContainsKey(elt.Id) || string.IsNullOrWhiteSpace((value = content[elt.Id].ToString()))))
                {
                    errors.Add("validation_error", $"field {elt.Id} is required");
                }

                switch (elt.Type)
                {
                    case FormElementTypes.INT:
                        int i;
                        if (!int.TryParse(value, out i))
                        {
                            errors.Add("validation_error", $"field {elt.Id} is not an integer");
                        }
                        break;
                    case FormElementTypes.BOOL:
                        bool b;
                        if (!bool.TryParse(value, out b))
                        {
                            errors.Add("validation_error", $"field {elt.Id} is not a boolean");
                        }
                        break;
                }

                result.Add(elt.Id, value);
            }

            if (errors.Any())
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = errors
                };
            }

            return result;
        }
    }
}
