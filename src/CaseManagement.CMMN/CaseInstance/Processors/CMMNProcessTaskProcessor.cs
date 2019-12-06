using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.Commands;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using Hangfire;
using System;
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

        public override string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.ProcessTask).ToLowerInvariant();
        
        public override async Task Run(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var planItem = context.GetCMMNPlanItem();
            var processTask = context.GetCMMNProcessTask();
            if (processTask.IsBlocking)
            {
                await HandleProcess(context, token);
            }
            else
            {
                BackgroundJob.Enqueue(() => HandleBackgroundProcess(context, token));
                pf.CompletePlanItem(planItem);
                await context.Complete(token);
            }
        }

        private async Task HandleBackgroundProcess(WorkflowHandlerContext context, CancellationToken token)
        {
            // TODO : SAVE THE MODIFICATIONS.
        }

        private async Task HandleProcess(WorkflowHandlerContext context, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var processTask = context.GetCMMNProcessTask();
            var parameters = new Dictionary<string, string>();
            var processRef = processTask.ProcessRef;
            if (string.IsNullOrWhiteSpace(processRef))
            {
                if (processTask.ProcessRefExpression == null)
                {
                    // TODO : THROW EXCEPTION.
                }

                processRef = ExpressionParser.GetStringEvaluation(processTask.ProcessRefExpression.Body, pf);
            }

            foreach (var mapping in processTask.Mappings)
            {
                if (!pf.ContainsVariable(mapping.SourceRef.Name))
                {
                    continue;
                }

                var variableValue = pf.GetVariable(mapping.SourceRef.Name);
                if (mapping.Transformation != null)
                {
                    variableValue = ExpressionParser.GetStringEvaluation(mapping.Transformation.Body, pf, (i) =>
                    {
                        i.SetVariable("sourceValue", variableValue);
                    });
                }

                parameters.Add(mapping.TargetRef.Name, variableValue);
            }

            await _caseLaunchProcessCommandHandler.Handle(new LaunchCaseProcessCommand(processRef, parameters), (resp) =>
            {
                return HandleLaunchCaseProcessCallback(context, resp, token);
            }, token);
        }

        private async Task HandleLaunchCaseProcessCallback(WorkflowHandlerContext context, CaseProcessResponse caseProcessResponse, CancellationToken token)
        {
            var pf = context.ProcessFlowInstance;
            var planItem = context.GetCMMNPlanItem();
            var processTask = context.GetCMMNProcessTask();
            foreach (var mapping in processTask.Mappings)
            {
                if (!caseProcessResponse.Parameters.ContainsKey(mapping.SourceRef.Name))
                {
                    continue;
                }

                string vv = caseProcessResponse.Parameters[mapping.SourceRef.Name];
                if (mapping.Transformation != null)
                {
                    vv = ExpressionParser.GetStringEvaluation(mapping.Transformation.Body, pf, (i) =>
                    {
                        i.SetVariable("sourceValue", vv);
                    });
                }

                pf.SetVariable(mapping.TargetRef.Name, vv);
            }

            pf.CompletePlanItem(planItem);
            await context.Complete(token);
        }
    }
}
