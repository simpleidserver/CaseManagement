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

            var processFlowInstance = BuildProcessFlowInstance(tCase, caseDefinition.id, caseDefinition);
            await _commitAggregateHelper.Commit(processFlowInstance, processFlowInstance.GetStreamName());
            return processFlowInstance;
        }

        public static ProcessFlowInstance BuildProcessFlowInstance(tCase tCase, string id, tDefinitions definitions)
        {
            var fileModel = tCase.caseFileModel;
            var planModel = tCase.casePlanModel;
            var flowInstanceElements = new List<CMMNPlanItem>();
            var caseFileItems = new List<CMMNCaseFileItem>();
            var connectors = new Dictionary<string, string>();
            if (fileModel != null && fileModel.caseFileItem.Any())
            {
                foreach (var fileItem in fileModel.caseFileItem)
                {
                    var caseFileItemDefinition = definitions.caseFileItemDefinition.First(c => c.id == fileItem.definitionRef.Name);
                    var caseFileItem = new CMMNCaseFileItem(fileItem.id, fileItem.name)
                    {
                        Multiplicity = (CMMNMultiplicities)fileItem.multiplicity,
                        Definition = new CMMNCaseFileItemDefinition(caseFileItemDefinition.definitionType)
                    };
                    caseFileItems.Add(caseFileItem);
                }
            }

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
                        var caseItemOnParts = sEntry.Items.Where(i => i is tCaseFileItemOnPart).Cast<tCaseFileItemOnPart>();
                        foreach(var planItemOnPart in planItemOnParts)
                        {
                            connectors.Add(planItemOnPart.sourceRef, planItem.id);
                        }

                        foreach(var caseItemOnPart in caseItemOnParts)
                        {
                            connectors.Add(caseItemOnPart.sourceRef, planItem.id);
                        }

                        flowInstanceElt.EntryCriterions.Add(new CMMNCriterion(entryCriterion.name) { SEntry = BuildSEntry(sEntry) });
                    }
                }

                if (planItem.itemControl != null && planItem.itemControl.manualActivationRule != null)
                {
                    var manualActivationRule = planItem.itemControl.manualActivationRule;
                    flowInstanceElt.ActivationRule = CMMNActivationRuleTypes.ManualActivation;
                    flowInstanceElt.ManualActivationRule = new CMMNManualActivationRule(manualActivationRule.name)
                    {
                        Expression = manualActivationRule.condition == null ? null : new CMMNExpression(manualActivationRule.condition.language, manualActivationRule.condition.Text.First())
                    };
                }

                if (planItem.itemControl != null && planItem.itemControl.repetitionRule != null)
                {
                    var repetitionRule = planItem.itemControl.repetitionRule;
                    flowInstanceElt.ActivationRule = CMMNActivationRuleTypes.Repetition;
                    flowInstanceElt.RepetitionRule = new CMMNRepetitionRule(repetitionRule.name)
                    {
                        Condition = repetitionRule.condition == null ? null : new CMMNExpression(repetitionRule.condition.language, repetitionRule.condition.Text.First())
                    };
                }

                flowInstanceElements.Add(flowInstanceElt);
            }

            var builder = CMMNProcessFlowInstanceBuilder.New(id, planModel.name);
            foreach(var flowInstance in flowInstanceElements)
            {
                builder.AddPlanItem(flowInstance);
            }

            foreach (var caseFileItem in caseFileItems)
            {
                builder.AddCaseFileItem(caseFileItem);
            }

            foreach (var connector in connectors)
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

            if (planItemDef is tMilestone)
            {
                return CMMNPlanItem.New(id, name, BuildMilestone((tMilestone)planItemDef));
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

            if (processTask.sourceRef != null)
            {
                result.SourceRef = processTask.sourceRef.Name;
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
            return new CMMNHumanTask(humanTask.name)
            {
                PerformerRef = humanTask.performerRef,
                FormId = humanTask.caseFormRef,
                IsBlocking = humanTask.isBlocking
            };
        }

        private static CMMNMilestone BuildMilestone(tMilestone milestone)
        {
            return new CMMNMilestone(milestone.name);
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
                        result.PlanItemOnParts.Add(new CMMNPlanItemOnPart
                        {
                            SourceRef = planItemOnPart.sourceRef,
                            StandardEvent = standardEvt
                        });
                    }
                    else if (onPart is tCaseFileItemOnPart)
                    {
                        var caseItemOnPart = onPart as tCaseFileItemOnPart;
                        var name = Enum.GetName(typeof(CaseFileItemTransition), caseItemOnPart.standardEvent);
                        var standardEvt = (CMMNCaseFileItemTransitions)Enum.Parse(typeof(CMMNCaseFileItemTransitions), name, true);
                        result.FileItemOnParts.Add(new CMMNCaseFileItemOnPart
                        {
                            SourceRef = caseItemOnPart.sourceRef,
                            StandardEvent = standardEvt
                        });
                    }
                }
            }

            return result;
        }
    }
}
