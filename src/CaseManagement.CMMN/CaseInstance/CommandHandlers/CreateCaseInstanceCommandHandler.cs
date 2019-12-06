using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class CreateCaseInstanceCommandHandler : ICreateCaseInstanceCommandHandler
    {
        private readonly ICMMNDefinitionsQueryRepository _cmmnDefinitionsQueryRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public CreateCaseInstanceCommandHandler(ICMMNDefinitionsQueryRepository cmmnDefinitionsQueryRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _cmmnDefinitionsQueryRepository = cmmnDefinitionsQueryRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public async Task<ProcessFlowInstance> Handle(CreateCaseInstanceCommand createCaseInstanceCommand)
        {
            var caseDefinition = await _cmmnDefinitionsQueryRepository.FindDefinitionById(createCaseInstanceCommand.CaseDefinitionId);
            if (caseDefinition == null)
            {
                // TODO : THROW EXCEPTION.
                return null;
            }

            var tCase = caseDefinition.@case.FirstOrDefault(c => c.id == createCaseInstanceCommand.CasesId);
            if (tCase == null)
            {
                // TODO : THROW EXCEPTION.
                return null;
            }

            var processFlowInstance = BuildProcessFlowInstance(tCase, caseDefinition.id);
            await _commitAggregateHelper.Commit(processFlowInstance, processFlowInstance.GetStreamName());
            return processFlowInstance;
        }

        public static ProcessFlowInstance BuildProcessFlowInstance(tCase tCase, string id)
        {
            var planModel = tCase.casePlanModel;
            var flowInstanceElements = new List<CMMNPlanItem>();
            var connectors = new Dictionary<string, string>();
            foreach(var planItem in planModel.planItem)
            {
                var planItemDef = planModel.Items.First(i => i.id == planItem.definitionRef);
                var flowInstanceElt = BuildPlanItem(planItem.id, planItem.name, planItemDef);
                if (planItem.entryCriterion != null)
                {
                    foreach(var entryCriterion in planItem.entryCriterion)
                    {
                        var sEntry = planModel.sentry.First(s => s.id == entryCriterion.sentryRef);
                        var planItemOnParts = sEntry.Items.Where(i => i is tPlanItemOnPart).Cast<tPlanItemOnPart>();
                        foreach(var planItemOnPart in planItemOnParts)
                        {
                            connectors.Add(planItemOnPart.sourceRef, planItem.id);
                        }

                        flowInstanceElt.EntryCriterions.Add(new CMMNCriterion(entryCriterion.name) { SEntry = BuildSEntry(sEntry) });
                    }
                }

                flowInstanceElements.Add(flowInstanceElt);
            }

            var builder = CMMNProcessFlowInstanceBuilder.New(id, planModel.name);
            foreach(var flowInstance in flowInstanceElements)
            {
                builder.AddPlanItem(flowInstance);
            }

            foreach(var connector in connectors)
            {
                builder.AddConnection(connector.Key, connector.Value);
            }

            return builder.Build();
        }

        private static CMMNPlanItem BuildPlanItem(string id, string name, tPlanItemDefinition planItemDef)
        {
            if (planItemDef is tProcessTask)
            {
                return CMMNPlanItem.New(id, name, BuildProcessTask((tProcessTask)planItemDef));
            }

            if (planItemDef is tHumanTask)
            {
                return CMMNPlanItem.New(id, name, BuildHumanTask((tHumanTask)planItemDef));
            }

            if (planItemDef is tTask)
            {
                return CMMNPlanItem.New(id, name, BuildTask((tTask)planItemDef));
            }

            if (planItemDef is tTimerEventListener)
            {
                return CMMNPlanItem.New(id, name, BuildTimerEventListener((tTimerEventListener)planItemDef));
            }

            return null;
        }

        private static CMMNTask BuildTask(tTask task)
        {
            return new CMMNTask(task.name);
        }

        private static CMMNProcessTask BuildProcessTask(tProcessTask processTask)
        {
            var result = new CMMNProcessTask(processTask.name) { IsBlocking = processTask.isBlocking };
            if (processTask.parameterMapping != null)
            {
                foreach(var pm in processTask.parameterMapping)
                {
                    result.Mappings.Add(new CMMNParameterMapping
                    {
                        SourceRef = new CMMNParameter { Name = pm.sourceRef },
                        TargetRef = new CMMNParameter { Name = pm.targetRef },
                        Transformation = pm.transformation == null  ? null : new CMMNExpression(pm.transformation.language)
                        {
                            Body = pm.transformation.Text == null ? null : pm.transformation.Text.First()
                        }
                    });
                }
            }

            if (processTask.processRef != null)
            {
                result.ProcessRef = processTask.processRef.Name;
            }

            if (processTask.processRefExpression != null)
            {
                result.ProcessRefExpression = new CMMNExpression(processTask.processRefExpression.language)
                {
                    Body = processTask.processRefExpression.Text.First()
                };
            }

            return result;
        }

        private static CMMNHumanTask BuildHumanTask(tHumanTask humanTask)
        {
            return new CMMNHumanTask(humanTask.name) { FormId = humanTask.caseFormRef, IsBlocking = humanTask.isBlocking };
        }

        private static CMMNTimerEventListener BuildTimerEventListener(tTimerEventListener timerEventListener)
        {
            var result = new CMMNTimerEventListener(timerEventListener.name);
            if (timerEventListener.timerExpression != null)
            {
                result.TimerExpression = new CMMNExpression(timerEventListener.timerExpression.language)
                {
                    Body = timerEventListener.timerExpression.Text.First()
                };
            }

            return result;
        }

        private static CMMNSEntry BuildSEntry(tSentry sEntry)
        {
            var result = new CMMNSEntry(sEntry.name);
            if (sEntry.ifPart != null)
            {
                result.IfPart = new CMMNIfPart
                {
                    Condition = sEntry.ifPart.condition.Text.First()
                };
            }

            if (sEntry.Items != null)
            {
                foreach(var onPart in sEntry.Items)
                {
                    if (onPart is tPlanItemOnPart)
                    {
                        var planItemOnPart = onPart as tPlanItemOnPart;
                        var name = Enum.GetName(typeof(PlanItemTransition), planItemOnPart.standardEvent);
                        var standardEvt = (CMMNPlanItemTransitions)Enum.Parse(typeof(CMMNPlanItemTransitions), name, true);
                        result.OnParts.Add(new CMMNPlanItemOnPart
                        {
                            SourceRef = planItemOnPart.sourceRef,
                            StandardEvent = standardEvt
                        });
                    }
                }
            }

            return result;
        }
    }
}
