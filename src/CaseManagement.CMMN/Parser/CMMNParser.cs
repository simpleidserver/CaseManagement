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
        public static ICollection<CMMNWorkflowDefinition> ExtractWorkflowDefinition(string path)
        {
            var cmmnTxt = File.ReadAllText(path);
            var fileName = Path.GetFileName(path);
            var result = new List<CMMNWorkflowDefinition>();
            var definitions = ParseWSDL(cmmnTxt);
            foreach(var cmmnCase in definitions.@case)
            {
                result.Add(BuildWorkflowDefinition(cmmnCase, fileName));
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

        private static CMMNWorkflowDefinition BuildWorkflowDefinition(tCase tCase, string fileName)
        {
            var planModel = tCase.casePlanModel;
            var planItems = BuildPlanItems(planModel);
            var builder = CMMNWorkflowBuilder.New(Guid.NewGuid().ToString(), tCase.name);
            foreach (var planItem in planItems)
            {
                builder.AddCMMNPlanItem(planItem);
            }

            var result = builder.Build();
            result.CmmnDefinition = fileName;
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
                foreach (var onPart in sEntry.Items)
                {
                    if (onPart is tPlanItemOnPart)
                    {
                        var planItemOnPart = onPart as tPlanItemOnPart;
                        var name = Enum.GetName(typeof(PlanItemTransition), planItemOnPart.standardEvent);
                        var standardEvt = (CMMNTransitions)Enum.Parse(typeof(CMMNTransitions), name, true);
                        result.PlanItemOnParts.Add(new CMMNPlanItemOnPart
                        {
                            SourceRef = planItemOnPart.sourceRef,
                            StandardEvent = standardEvt
                        });
                    }
                }
            }

            return result;
        }

        private static CMMNPlanItemDefinition BuildPlanItem(string id, string name, tPlanItemDefinition planItemDef)
        {
            if (planItemDef is tProcessTask)
            {
                return CMMNPlanItemDefinition.New(id, name, BuildProcessTask((tProcessTask)planItemDef));
            }

            if (planItemDef is tHumanTask)
            {
                return CMMNPlanItemDefinition.New(id, name, BuildHumanTask((tHumanTask)planItemDef));
            }

            if (planItemDef is tTask)
            {
                return CMMNPlanItemDefinition.New(id, name, BuildTask((tTask)planItemDef));
            }

            if (planItemDef is tTimerEventListener)
            {
                return CMMNPlanItemDefinition.New(id, name, BuildTimerEventListener((tTimerEventListener)planItemDef));
            }

            if (planItemDef is tMilestone)
            {
                return CMMNPlanItemDefinition.New(id, name, BuildMilestone((tMilestone)planItemDef));
            }

            if (planItemDef is tStage)
            {
                return CMMNPlanItemDefinition.New(id, name, BuildStage((tStage)planItemDef));
            }

            return null;
        }

        private static CMMNProcessTask BuildProcessTask(tProcessTask processTask)
        {
            var result = new CMMNProcessTask(processTask.name) { IsBlocking = processTask.isBlocking };
            if (processTask.parameterMapping != null)
            {
                foreach (var pm in processTask.parameterMapping)
                {
                    result.Mappings.Add(new CMMNParameterMapping
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

        private static CMMNHumanTask BuildHumanTask(tHumanTask humanTask)
        {
            return new CMMNHumanTask(humanTask.name) { FormId = humanTask.caseFormRef, IsBlocking = humanTask.isBlocking };
        }

        private static CMMNTask BuildTask(tTask task)
        {
            return new CMMNTask(task.name);
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

        private static CMMNMilestone BuildMilestone(tMilestone milestone)
        {
            return new CMMNMilestone(milestone.name);
        }

        private static CMMNStageDefinition BuildStage(tStage stage)
        {
            var planItems = BuildPlanItems(stage);
            var result = new CMMNStageDefinition(stage.name);
            foreach (var planItem in planItems)
            {
                result.Elements.Add(planItem);
            }

            return result;
        }

        private static List<CMMNPlanItemDefinition> BuildPlanItems(tStage stage)
        {
            var planItems = new List<CMMNPlanItemDefinition>();
            foreach (var planItem in stage.planItem)
            {
                var planItemDef = stage.Items.First(i => i.id == planItem.definitionRef);
                var flowInstanceElt = BuildPlanItem(planItem.id, planItem.name, planItemDef);
                if (planItem.entryCriterion != null)
                {
                    foreach (var entryCriterion in planItem.entryCriterion)
                    {
                        var sEntry = stage.sentry.First(s => s.id == entryCriterion.sentryRef);
                        flowInstanceElt.EntryCriterions.Add(new CMMNCriterion(entryCriterion.name) { SEntry = BuildSEntry(sEntry) });
                    }
                }

                if (planItem.exitCriterion != null)
                {
                    foreach (var exitCriteria in planItem.exitCriterion)
                    {
                        var sEntry = stage.sentry.First(s => s.id == exitCriteria.sentryRef);
                        flowInstanceElt.ExitCriterions.Add(new CMMNCriterion(exitCriteria.name) { SEntry = BuildSEntry(sEntry) });
                    }
                }

                if (planItem.itemControl != null)
                {
                    if (planItem.itemControl.manualActivationRule != null)
                    {
                        var manualActivationRule = new CMMNManualActivationRule(planItem.itemControl.manualActivationRule.name);
                        var condition = planItem.itemControl.manualActivationRule.condition;
                        if (condition != null && condition.Text.Any())
                        {
                            manualActivationRule.Expression = new CMMNExpression(condition.language, condition.Text.First());
                        }

                        flowInstanceElt.SetManualActivationRule(manualActivationRule);
                    }
                    else if (planItem.itemControl.repetitionRule != null)
                    {
                        var repetitionRule = new CMMNRepetitionRule(planItem.itemControl.repetitionRule.name);
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
