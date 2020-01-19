using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.Commands;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using System.Collections.Generic;
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

        public override CaseElementTypes Type => CaseElementTypes.ProcessTask;

        protected override async Task Run(ProcessorParameter parameter, CancellationToken token)
        {
            var planItem = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId) as PlanItemDefinition;
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

        private async Task HandleProcess(ProcessorParameter parameter, CancellationToken token)
        {
            var planItem = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId) as PlanItemDefinition;
            var processTask = planItem.PlanItemDefinitionProcessTask;
            var parameters = new Dictionary<string, string>();
            var processRef = processTask.ProcessRef;
            if (string.IsNullOrWhiteSpace(processRef))
            {
                processRef = ExpressionParser.GetStringEvaluation(processTask.ProcessRefExpression.Body, parameter.CaseInstance);
            }

            foreach (var mapping in processTask.Mappings)
            {

                string variableValue;
                if (mapping.SourceRef.Name == CMMNConstants.StandardProcessMappingVariables.CaseInstanceId)
                {
                    variableValue = parameter.CaseInstance.Id;
                }
                else
                {
                    if (!parameter.CaseInstance.ContainsVariable(mapping.SourceRef.Name))
                    {
                        continue;
                    }

                    variableValue = parameter.CaseInstance.GetVariable(mapping.SourceRef.Name);
                    if (mapping.Transformation != null)
                    {
                        variableValue = ExpressionParser.GetStringEvaluation(mapping.Transformation.Body, parameter.CaseInstance, (i) =>
                        {
                            i.SetVariable("sourceValue", variableValue);
                        });
                    }
                }

                parameters.Add(mapping.TargetRef.Name, variableValue);
            }
            
            await _caseLaunchProcessCommandHandler.Handle(new LaunchCaseProcessCommand(processRef, parameters), (resp) =>
            {
                return HandleLaunchCaseProcessCallback(parameter, resp, token);
            }, token);
        }
        
        private Task HandleLaunchCaseProcessCallback(ProcessorParameter parameter, CaseProcessResponse caseProcessResponse, CancellationToken token)
        {
            var planItem = parameter.CaseDefinition.GetElement(parameter.CaseElementInstance.CaseElementDefinitionId) as PlanItemDefinition;
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
                    vv = ExpressionParser.GetStringEvaluation(mapping.Transformation.Body, parameter.CaseInstance, (i) =>
                    {
                        i.SetVariable("sourceValue", vv);
                    });
                }

                parameter.CaseInstance.SetVariable(mapping.TargetRef.Name, vv);
            }

            parameter.CaseInstance.MakeTransitionComplete(parameter.CaseElementInstance.Id);
            return Task.CompletedTask;
        }
    }
}
