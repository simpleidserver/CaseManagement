using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CasePlan;
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

        public static ICollection<CasePlanAggregate> ExtractCasePlans(tDefinitions definitions, CaseFileAggregate caseFile)
        {
            var result = new List<CasePlanAggregate>();
            foreach (var cmmnCase in definitions.@case)
            {
                result.Add(BuildCasePlan(cmmnCase, caseFile));
            }

            return result;
        }
        
        public static StageElementInstance ExtractStage(string xmlContent)
        {
            var tStage = DeserializeStage(xmlContent);
            var result = BuildStage(tStage);
            return result;
        }

        private static CasePlanAggregate BuildCasePlan(tCase tCase, CaseFileAggregate caseFile)
        {
            var planModel = tCase.casePlanModel;
            var roles = new List<CasePlanRole>();
            if (tCase.caseRoles != null && tCase.caseRoles.role != null)
            {
                foreach(var role in tCase.caseRoles.role)
                {
                    roles.Add(new CasePlanRole
                    {
                        Id = role.id,
                        Name = role.name
                    });
                }
            }

            return CasePlanAggregate.New(planModel.id, planModel.name, planModel.name, caseFile.Owner, caseFile.Id, caseFile.Version, Serialize(planModel), roles);
        }

        private static StageElementInstance BuildStage(tStage stage)
        {
            var planItems = BuildPlanItems(stage);
            var result = new StageElementInstance { Id = stage.id, Name = stage.name };
            foreach (var planItem in planItems)
            {
                result.AddChild(planItem);
            }

            return result;
        }

        private static List<CasePlanElementInstance> BuildPlanItems(tStage stage)
        {
            var planItems = new List<CasePlanElementInstance>();
            if (stage.planItem != null)
            {
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
                        var baseTask = flowInstanceElt as BaseTaskOrStageElementInstance;
                        if (planItem.itemControl.manualActivationRule != null && baseTask != null)
                        {
                            var manualActivationRule = new ManualActivationRule(planItem.itemControl.manualActivationRule.name);
                            var condition = planItem.itemControl.manualActivationRule.condition;
                            if (condition != null && condition.Text.Any())
                            {
                                manualActivationRule.Expression = new CMMNExpression(condition.language, condition.Text.First());
                            }

                            baseTask.ManualActivationRule = manualActivationRule;
                        }

                        if (planItem.itemControl.repetitionRule != null)
                        {
                            var repetitionRule = new RepetitionRule(planItem.itemControl.repetitionRule.name);
                            var condition = planItem.itemControl.repetitionRule.condition;
                            if (condition != null && condition.Text.Any())
                            {
                                repetitionRule.Condition = new CMMNExpression(condition.language, condition.Text.First());
                            }

                            flowInstanceElt.RepetitionRule = repetitionRule;
                        }
                    }

                    planItems.Add(flowInstanceElt);
                }
            }

            return planItems;
        }

        private static CasePlanElementInstance BuildPlanItem(string id, string name, tPlanItemDefinition planItemDef)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = planItemDef.name;
            }

            if (planItemDef is tHumanTask)
            {
                var humanTask = planItemDef as tHumanTask;
                return new HumanTaskElementInstance { Id = id, Name = name, FormId = humanTask.caseFormRef, PerformerRef = humanTask.performerRef };
            }

            if (planItemDef is tTask)
            {
                return new EmptyTaskElementInstance { Id = id, Name = name };
            }

            if (planItemDef is tTimerEventListener)
            {
                var timer = planItemDef as tTimerEventListener;
                CMMNExpression expression = new CMMNExpression
                {
                    Body = timer.timerExpression.Text.First(),
                    Language = timer.timerExpression.language
                };
                return new TimerEventListener { Id = id, Name = name, TimerExpression = expression };
            }

            if (planItemDef is tMilestone)
            {
                return new MilestoneElementInstance { Id = id, Name = name };
            }

            if (planItemDef is tStage)
            {
                // return PlanItemDefinition.New(id, name, BuildStage((tStage)planItemDef));
            }

            return null;
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

        private static tStage DeserializeStage(string xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(tStage));
            tStage result = null;
            using (var txtReader = new StringReader(xml))
            {
                result = (tStage)xmlSerializer.Deserialize(txtReader);
            }

            return result;
        }

        private static string Serialize(tStage stage)
        {
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            var strBuilder = new StringBuilder();
            using (var txtWriter = new StringWriter(strBuilder))
            {
                xmlSerializer.Serialize(txtWriter, stage);
            }

            return strBuilder.ToString();
        }
    }
}
