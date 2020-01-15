using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.CaseInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
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
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly ICaseDefinitionQueryRepository _cmmnWorkflowDefinitionQueryRepository;

        public ConfirmFormCommandHandler(IQueueProvider queueProvider, IFormQueryRepository formQueryRepository, IEventStoreRepository eventStoreRepository, IRoleQueryRepository roleQueryRepository, ICaseDefinitionQueryRepository cmmnWorkflowDefinitionQueryRepository)
        {
            _queueProvider = queueProvider;
            _formQueryRepository = formQueryRepository;
            _eventStoreRepository = eventStoreRepository;
            _roleQueryRepository = roleQueryRepository;
            _cmmnWorkflowDefinitionQueryRepository = cmmnWorkflowDefinitionQueryRepository;
        }
        
        public async Task Handle(ConfirmFormCommand confirmFormCommand)
        {
            var caseInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(confirmFormCommand.CaseInstanceId, Domains.CaseInstance.GetStreamName(confirmFormCommand.CaseInstanceId));
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.Id))
            {
                throw new UnknownCaseInstanceException(confirmFormCommand.CaseInstanceId);
            }
            
            var caseInstanceElt = caseInstance.WorkflowElementInstances.FirstOrDefault(e => e.Id == confirmFormCommand.CaseElementInstanceId);
            if (caseInstanceElt == null)
            {
                throw new UnknownCaseInstanceElementException(caseInstance.Id, confirmFormCommand.CaseElementInstanceId);
            }
            
            if (caseInstanceElt.CaseElementDefinitionType != CaseElementTypes.HumanTask)
            {
                throw new NotSupportedTaskException("Task must be a HumanTask");
            }

            var workflowDefinition = await _cmmnWorkflowDefinitionQueryRepository.FindById(caseInstance.CaseDefinitionId);
            var humanTask = (workflowDefinition.GetElement(caseInstanceElt.CaseElementDefinitionId) as PlanItemDefinition).PlanItemDefinitionHumanTask;
            if (!string.IsNullOrWhiteSpace(humanTask.PerformerRef))
            {
                var roles = await _roleQueryRepository.FindRolesByUser(confirmFormCommand.UserIdentifier);
                if (!roles.Any(r => r.Name == humanTask.PerformerRef))
                {
                    throw new UnauthorizedCaseWorkerException(confirmFormCommand.UserIdentifier, caseInstance.Id, caseInstanceElt.Id);
                }
            }

            var form = await _formQueryRepository.FindFormById(humanTask.FormId);
            if (form == null)
            {
                throw new UnknownFormException(humanTask.FormId);
            }

            var formValues = CheckConfirmForm(form, confirmFormCommand.Content);
            caseInstance.SubmitForm(caseInstanceElt.Id, caseInstanceElt.FormInstanceId, formValues);
            await _queueProvider.QueueSubmitForm(caseInstance.Id, caseInstanceElt.Id, caseInstanceElt.FormInstanceId, formValues);
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
                throw new AggregateValidationException(errors);
            }

            return result;
        }
    }
}
