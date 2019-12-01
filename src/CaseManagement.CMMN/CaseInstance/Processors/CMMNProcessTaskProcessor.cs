using CaseManagement.CMMN.CaseProcess.CommandHandlers;
using CaseManagement.CMMN.CaseProcess.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNProcessTaskProcessor : BaseCMMNTaskProcessor
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private ICaseLaunchProcessCommandHandler _caseLaunchProcessCommandHandler;

        public CMMNProcessTaskProcessor(IBackgroundTaskQueue backgroundTaskQueue, ICaseLaunchProcessCommandHandler caseLaunchProcessCommandHandler)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _caseLaunchProcessCommandHandler = caseLaunchProcessCommandHandler;
        }

        public override Type PlanItemDefinitionType => typeof(CMMNProcessTask);

        public override async Task<bool> Run(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf)
        {
            var processTask = cmmnPlanItem.PlanItemDefinition as CMMNProcessTask;
            if (processTask.IsBlocking)
            {
                await RunProcess(processTask, pf);
            }
            else
            {
                _backgroundTaskQueue.QueueBackgroundWorkItem((token) => RunProcess(processTask, pf));
            }

            cmmnPlanItem.Complete();
            return true;
        }

        private async Task RunProcess(CMMNProcessTask processTask, ProcessFlowInstance processFlowInstance)
        {
            var parameters = new Dictionary<string, string>();
            var processRef = processTask.ProcessRef;
            if (string.IsNullOrWhiteSpace(processRef))
            {
                if (processTask.ProcessRefExpression == null)
                {
                    // TODO : THROW EXCEPTION.
                }

                // TODO : Evaluate the ProcessRefExpression to get the processRefName.
            }

            foreach(var mapping in processTask.Mappings)
            {
                if (!processFlowInstance.ContainsVariable(mapping.SourceRef.Name))
                {
                    continue;
                }

                var variableValue = processFlowInstance.GetVariable(mapping.SourceRef.Name);
                if (mapping.Transformation != null)
                {
                    // TODO : EVALUATE THE TRANSFORMATION TO GET THE TARGET VALUE.
                }

                parameters.Add(mapping.TargetRef.Name, variableValue);
            }

            var result = await _caseLaunchProcessCommandHandler.Handle(new LaunchCaseProcessCommand
            {
                Id = processTask.ProcessRef,
                Parameters = parameters
            });
            foreach(var mapping in processTask.Mappings)
            {
                if (!result.Parameters.ContainsKey(mapping.SourceRef.Name))
                {
                    continue;
                }

                var variableValue = result.Parameters[mapping.SourceRef.Name];
                if (mapping.Transformation != null)
                {
                    // TODO : EVALUATE THE TRANSFORMATION TO GET THE TARGET VALUE.
                }

                processFlowInstance.SetVariable(mapping.TargetRef.Name, variableValue);
            }
        }
    }
}
