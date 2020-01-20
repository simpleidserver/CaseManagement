using CaseManagement.CMMN.Builders;
using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CaseManagement.CMMN.Parser
{
    public class CMMNParser
    {
        public static ICollection<CaseDefinition> ExtractWorkflowDefinition(string path)
        {
            var cmmnTxt = File.ReadAllText(path);
            var fileName = Path.GetFileName(path);
            var result = new List<CaseDefinition>();
            var definitions = ParseWSDL(cmmnTxt);
            foreach(var cmmnCase in definitions.@case)
            {
                result.Add(BuildWorkflowDefinition(cmmnCase, definitions, fileName));
            }
            
            return result;
        }

        public static tDefinitions ParseWSDL(string cmmnTxt)
        {
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            tDefinitions defs = null;
            using (var txtReader = new StringReader(cmmnTxt))
            {
                defs = (tDefinitions)xmlSerializer.Deserialize(txtReader);
            }

            return defs;
        }

        public static string Serialize(tDefinitions def)
        {
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            var strBuilder = new StringBuilder();
            using (var txtWriter = new StringWriter(strBuilder))
            {
                xmlSerializer.Serialize(txtWriter, def);
            }

            return strBuilder.ToString();
        }

        private static CaseDefinition BuildWorkflowDefinition(tCase tCase, tDefinitions definitions, string caseFileId)
        {
            var planModel = tCase.casePlanModel;
            var planItems = BuildPlanItems(planModel);
            var builder = WorkflowBuilder.New(Guid.NewGuid().ToString(), tCase.casePlanModel.name);
            if (tCase.caseFileModel != null)
            {
                foreach(var caseFileItem in tCase.caseFileModel.caseFileItem)
                {
                    var caseFileItemDef = definitions.caseFileItemDefinition.First(c => c.id == caseFileItem.definitionRef.ToString());
                    builder.AddCaseFileItem(caseFileItem.id, caseFileItemDef.name, (cb) =>
                    {
                        cb.SetDefinition(caseFileItemDef.definitionType);
                    });
                }
            }

            foreach (var planItem in planItems)
            {
                builder.AddCMMNPlanItem(planItem);
            }

            var result = builder.Build();
            result.CaseFileId = caseFileId;
            return result;
        }
        
        private static SEntry BuildSEntry(tSentry sEntry)
        {
            var result = new SEntry(sEntry.name);
            if (sEntry.ifPart != null)
            {
                result.IfPart = new IfPart
                {
                    Condition = sEntry.ifPart.condition.Text.First()
                };
            }

            if (sEntry.Items != null)
            {
                foreach (var onPart in sEntry.Items)
                {
                    if (onPart is tPlanItemOnPart)
                    {
                        var planItemOnPart = onPart as tPlanItemOnPart;
                        var name = Enum.GetName(typeof(PlanItemTransition), planItemOnPart.standardEvent);
                        var standardEvt = (CMMNTransitions)Enum.Parse(typeof(CMMNTransitions), name, true);
                        result.PlanItemOnParts.Add(new PlanItemOnPart
                        {
                            SourceRef = planItemOnPart.sourceRef,
                            StandardEvent = standardEvt
                        });
                    }
                    else if (onPart is tCaseFileItemOnPart)
                    {
                        var caseFileItemOnPart = onPart as tCaseFileItemOnPart;
                        var name = Enum.GetName(typeof(CaseFileItemTransition), caseFileItemOnPart.standardEvent);
                        var standardEvt = (CMMNTransitions)Enum.Parse(typeof(CMMNTransitions), name, true);
                        result.FileItemOnParts.Add(new CaseFileItemOnPart
                        {
                            SourceRef = caseFileItemOnPart.sourceRef,
                            StandardEvent = standardEvt
                        });
                    }
                }
            }

            return result;
        }

        private static PlanItemDefinition BuildPlanItem(string id, string name, tPlanItemDefinition planItemDef)
        {
            if (planItemDef is tProcessTask)
            {
                return PlanItemDefinition.New(id, name, BuildProcessTask((tProcessTask)planItemDef));
            }

            if (planItemDef is tHumanTask)
            {
                return PlanItemDefinition.New(id, name, BuildHumanTask((tHumanTask)planItemDef));
            }

            if (planItemDef is tTask)
            {
                return PlanItemDefinition.New(id, name, BuildTask((tTask)planItemDef));
            }

            if (planItemDef is tTimerEventListener)
            {
                return PlanItemDefinition.New(id, name, BuildTimerEventListener((tTimerEventListener)planItemDef));
            }

            if (planItemDef is tMilestone)
            {
                return PlanItemDefinition.New(id, name, BuildMilestone((tMilestone)planItemDef));
            }

            if (planItemDef is tStage)
            {
                return PlanItemDefinition.New(id, name, BuildStage((tStage)planItemDef));
            }

            return null;
        }

        private static ProcessTask BuildProcessTask(tProcessTask processTask)
        {
            var result = new ProcessTask(processTask.name) { IsBlocking = processTask.isBlocking };
            if (processTask.parameterMapping != null)
            {
                foreach (var pm in processTask.parameterMapping)
                {
                    result.Mappings.Add(new ParameterMapping
                    {
                        SourceRef = new CMMNParameter { Name = pm.sourceRef },
                        TargetRef = new CMMNParameter { Name = pm.targetRef },
                        Transformation = pm.transformation == null ? null : new CMMNExpression(pm.transformation.language)
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

        private static HumanTask BuildHumanTask(tHumanTask humanTask)
        {
            return new HumanTask(humanTask.name) { FormId = humanTask.caseFormRef, IsBlocking = humanTask.isBlocking, PerformerRef = humanTask.performerRef };
        }

        private static CMMNTask BuildTask(tTask task)
        {
            return new CMMNTask(task.name);
        }

        private static TimerEventListener BuildTimerEventListener(tTimerEventListener timerEventListener)
        {
            var result = new TimerEventListener(timerEventListener.name);
            if (timerEventListener.timerExpression != null)
            {
                result.TimerExpression = new CMMNExpression(timerEventListener.timerExpression.language)
                {
                    Body = timerEventListener.timerExpression.Text.First()
                };
            }

            return result;
        }

        private static Milestone BuildMilestone(tMilestone milestone)
        {
            return new Milestone(milestone.name);
        }

        private static StageDefinition BuildStage(tStage stage)
        {
            var planItems = BuildPlanItems(stage);
            var result = new StageDefinition(stage.name);
            foreach (var planItem in planItems)
            {
                result.Elements.Add(planItem);
            }

            return result;
        }

        private static List<PlanItemDefinition> BuildPlanItems(tStage stage)
        {
            var planItems = new List<PlanItemDefinition>();
            foreach (var planItem in stage.planItem)
            {
                var planItemDef = stage.Items.First(i => i.id == planItem.definitionRef);
                var flowInstanceElt = BuildPlanItem(planItem.id, planItem.name, planItemDef);
                if (planItem.entryCriterion != null)
                {
                    foreach (var entryCriterion in planItem.entryCriterion)
                    {
                        var sEntry = stage.sentry.First(s => s.id == entryCriterion.sentryRef);
                        flowInstanceElt.EntryCriterions.Add(new Criteria(entryCriterion.name) { SEntry = BuildSEntry(sEntry) });
                    }
                }

                if (planItem.exitCriterion != null)
                {
                    foreach (var exitCriteria in planItem.exitCriterion)
                    {
                        var sEntry = stage.sentry.First(s => s.id == exitCriteria.sentryRef);
                        flowInstanceElt.ExitCriterions.Add(new Criteria(exitCriteria.name) { SEntry = BuildSEntry(sEntry) });
                    }
                }

                if (planItem.itemControl != null)
                {
                    if (planItem.itemControl.manualActivationRule != null)
                    {
                        var manualActivationRule = new ManualActivationRule(planItem.itemControl.manualActivationRule.name);
                        var condition = planItem.itemControl.manualActivationRule.condition;
                        if (condition != null && condition.Text.Any())
                        {
                            manualActivationRule.Expression = new CMMNExpression(condition.language, condition.Text.First());
                        }

                        flowInstanceElt.SetManualActivationRule(manualActivationRule);
                    }
                    else if (planItem.itemControl.repetitionRule != null)
                    {
                        var repetitionRule = new RepetitionRule(planItem.itemControl.repetitionRule.name);
                        var condition = planItem.itemControl.repetitionRule.condition;
                        if (condition != null && condition.Text.Any())
                        {
                            repetitionRule.Condition = new CMMNExpression(condition.language, condition.Text.First());
                        }

                        flowInstanceElt.SetRepetitionRule(repetitionRule);
                    }
                }

                planItems.Add(flowInstanceElt);
            }

            return planItems;
        }
    }
}
