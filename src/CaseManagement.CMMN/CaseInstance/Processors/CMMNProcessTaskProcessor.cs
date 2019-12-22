using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.Commands;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNProcessTaskProcessor : BaseCMMNTaskProcessor
    {
        private ICaseLaunchProcessCommandHandler _caseLaunchProcessCommandHandler;

        public CMMNProcessTaskProcessor(ICaseLaunchProcessCommandHandler caseLaunchProcessCommandHandler)
        {
            _caseLaunchProcessCommandHandler = caseLaunchProcessCommandHandler;
        }

        public override CMMNWorkflowElementTypes Type => CMMNWorkflowElementTypes.ProcessTask;

        protected override async Task Run(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var planItem = parameter.WorkflowDefinition.GetElement(parameter.WorkflowElementInstance.WorkflowElementDefinitionId) as CMMNPlanItemDefinition;
            var processTask = planItem.PlanItemDefinitionProcessTask;
            if (processTask.IsBlocking)
            {
                await HandleProcess(parameter, token);
            }
            else
            {
                // TODO : ADD BACKGROUND PROCESS.
            }
        }

        protected override void Unsubscribe()
        {

        }

        private async Task HandleProcess(PlanItemProcessorParameter parameter, CancellationToken token)
        {
            var planItem = parameter.WorkflowDefinition.GetElement(parameter.WorkflowElementInstance.WorkflowElementDefinitionId) as CMMNPlanItemDefinition;
            var processTask = planItem.PlanItemDefinitionProcessTask;
            var parameters = new Dictionary<string, string>();
            var processRef = processTask.ProcessRef;
            if (string.IsNullOrWhiteSpace(processRef))
            {
                processRef = ExpressionParser.GetStringEvaluation(processTask.ProcessRefExpression.Body, parameter.WorkflowInstance);
            }

            foreach (var mapping in processTask.Mappings)
            {
                if (!parameter.WorkflowInstance.ContainsVariable(mapping.SourceRef.Name))
                {
                    continue;
                }

                var variableValue = parameter.WorkflowInstance.GetVariable(mapping.SourceRef.Name);
                if (mapping.Transformation != null)
                {
                    variableValue = ExpressionParser.GetStringEvaluation(mapping.Transformation.Body, parameter.WorkflowInstance, (i) =>
                    {
                        i.SetVariable("sourceValue", variableValue);
                    });
                }

                parameters.Add(mapping.TargetRef.Name, variableValue);
            }

            if (!string.IsNullOrWhiteSpace(processTask.SourceRef))
            {
                // var caseFileItem = pf.GetCaseFileItem(processTask.SourceRef);
                // parameters.Add("caseFileItem", JsonConvert.SerializeObject(caseFileItem));
            }
            
            await _caseLaunchProcessCommandHandler.Handle(new LaunchCaseProcessCommand(processRef, parameters), (resp) =>
            {
                return HandleLaunchCaseProcessCallback(parameter, resp, token);
            }, token);
        }
        
        private Task HandleLaunchCaseProcessCallback(PlanItemProcessorParameter parameter, CaseProcessResponse caseProcessResponse, CancellationToken token)
        {
            var planItem = parameter.WorkflowDefinition.GetElement(parameter.WorkflowElementInstance.WorkflowElementDefinitionId) as CMMNPlanItemDefinition;
            var processTask = planItem.PlanItemDefinitionProcessTask;
            foreach (var mapping in processTask.Mappings)
            {
                if (!caseProcessResponse.Parameters.ContainsKey(mapping.SourceRef.Name))
                {
                    continue;
                }

                string vv = caseProcessResponse.Parameters[mapping.SourceRef.Name];
                if (mapping.Transformation != null)
                {
                    vv = ExpressionParser.GetStringEvaluation(mapping.Transformation.Body, parameter.WorkflowInstance, (i) =>
                    {
                        i.SetVariable("sourceValue", vv);
                    });
                }

                parameter.WorkflowInstance.SetVariable(mapping.TargetRef.Name, vv);
            }
            
            parameter.WorkflowInstance.MakeTransition(parameter.WorkflowElementInstance.Id, CMMNTransitions.Complete);
            return Task.CompletedTask;
        }
    }
}
