using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
                result.Add(BuildCasePlan(cmmnCase, definitions, caseFile));
            }

            return result;
        }

        public static CaseEltInstance ExtractStage(string xmlContent, string casePlanInstanceId)
        {
            var tStage = DeserializeStage(xmlContent);
            var result = BuildStage(tStage.id, tStage, casePlanInstanceId);
            return result;
        }

        private static CasePlanAggregate BuildCasePlan(tCase tCase, tDefinitions definitions, CaseFileAggregate caseFile)
        {
            var planModel = tCase.casePlanModel;
            var roles = new List<CasePlanRole>();
            var files = new List<CasePlanFileItem>();
            if (tCase.caseRoles != null && tCase.caseRoles.role != null)
            {
                foreach (var role in tCase.caseRoles.role)
                {
                    roles.Add(new CasePlanRole
                    {
                        EltId = role.id,
                        Name = role.name
                    });
                }
            }

            if (tCase.caseFileModel != null && tCase.caseFileModel.caseFileItem != null)
            {
                foreach(var caseFileItem in tCase.caseFileModel.caseFileItem)
                {
                    var caseFileItemDef = definitions.caseFileItemDefinition.First(c => c.id == caseFileItem.definitionRef.ToString());
                    files.Add(new CasePlanFileItem
                    {
                        DefinitionType = caseFileItemDef.definitionType,
                        EltId = caseFileItem.id,
                        Name = caseFileItem.name
                    });
                }
            }

            return CasePlanAggregate.New(planModel.id, planModel.name, planModel.name, caseFile.AggregateId, caseFile.Version, Serialize(planModel), roles, files);
        }

        private static CaseEltInstance BuildStage(string id, tStage stage, string casePlanInstanceId)
        {
            var planItems = BuildPlanItems(stage, casePlanInstanceId);
            var result = new CaseEltInstance
            { 
                Id = CaseEltInstance.BuildId(casePlanInstanceId, stage.id, 0),
                Name = stage.name,
                EltId = id,
                Type = CasePlanElementInstanceTypes.STAGE
            };
            foreach (var planItem in planItems)
            {
                result.AddChild(planItem);
            }
            
            if (!result.ExitCriterions.Any() && stage.exitCriterion != null)
            {
                foreach (var exitCriteria in stage.exitCriterion)
                {
                    var sEntry = stage.sentry.First(s => s.id == exitCriteria.sentryRef);
                    result.AddExitCriteria(new Criteria(exitCriteria.name) { SEntry = BuildSEntry(sEntry) });
                }
            }

            return result;
        }

        private static List<CaseEltInstance> BuildPlanItems(tStage stage, string casePlanInstanceId)
        {
            var planItems = new List<CaseEltInstance>();
            if (stage.planItem != null)
            {
                foreach (var planItem in stage.planItem)
                {
                    var planItemDef = stage.Items.First(i => i.id == planItem.definitionRef);
                    var flowInstanceElt = BuildPlanItem(planItem.id, planItem.name, planItemDef, casePlanInstanceId);
                    if (planItem.entryCriterion != null)
                    {
                        foreach (var entryCriterion in planItem.entryCriterion)
                        {
                            var sEntry = stage.sentry.First(s => s.id == entryCriterion.sentryRef);
                            flowInstanceElt.AddEntryCriteria(new Criteria(entryCriterion.name) { SEntry = BuildSEntry(sEntry) });
                        }
                    }

                    if (planItem.exitCriterion != null)
                    {
                        foreach (var exitCriteria in planItem.exitCriterion)
                        {
                            var sEntry = stage.sentry.First(s => s.id == exitCriteria.sentryRef);
                            flowInstanceElt.AddExitCriteria(new Criteria(exitCriteria.name) { SEntry = BuildSEntry(sEntry) });
                        }
                    }

                    if (planItem.itemControl != null)
                    {
                        if (planItem.itemControl.manualActivationRule != null && flowInstanceElt.IsTaskOrStage())
                        {
                            var manualActivationRule = new ManualActivationRule(planItem.itemControl.manualActivationRule.name);
                            var condition = planItem.itemControl.manualActivationRule.condition;
                            if (condition != null && condition.Text.Any())
                            {
                                manualActivationRule.Expression = new CMMNExpression(condition.language, condition.Text.First());
                            }

                            flowInstanceElt.ManualActivationRule = manualActivationRule;
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

        private static CaseEltInstance BuildPlanItem(string id, string name, tPlanItemDefinition planItemDef, string casePlanInstanceId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = planItemDef.name;
            }

            if (planItemDef is tHumanTask)
            {
                var humanTask = planItemDef as tHumanTask;
                List<tHumanTaskParameter> pars = new List<tHumanTaskParameter>();
                if (humanTask.implementation == CMMNConstants.UserTaskImplementations.WSHUMANTASK)
                {
                    var parameters = humanTask.extensionElements?.Any.FirstOrDefault(_ => _.Name == "cmg:parameters");
                    if (parameters != null)
                    {
                        var xmlSerializer = new XmlSerializer(typeof(tHumanTaskParameter), "https://github.com/simpleidserver/CaseManagement");
                        foreach (XmlNode child in parameters.ChildNodes)
                        {
                            using (var txtReader = new StringReader(child.OuterXml))
                            {
                                pars.Add((tHumanTaskParameter)xmlSerializer.Deserialize(txtReader));
                            }
                        }
                    }
                }

                var result = new CaseEltInstance
                { 
                    Id = CaseEltInstance.BuildId(casePlanInstanceId, id, 0),
                    EltId = id,
                    NbOccurrence = 0,
                    Name = name, 
                    Type = CasePlanElementInstanceTypes.HUMANTASK
                };
                result.UpdateFormId(humanTask.formId);
                result.UpdateImplementation(humanTask.implementation);
                result.UpdateInputParameters(pars.ToDictionary(kvp => kvp.key, kvp => kvp.value));
                result.UpdatePerformerRef(humanTask.performerRef);
                return result;
            }

            if (planItemDef is tTask)
            {
                return new CaseEltInstance
                {
                    Id = CaseEltInstance.BuildId(casePlanInstanceId, id, 0),
                    EltId = id,
                    NbOccurrence = 0,
                    Name = name,
                    Type = CasePlanElementInstanceTypes.EMPTYTASK
                };
            }

            if (planItemDef is tTimerEventListener)
            {
                var timer = planItemDef as tTimerEventListener;
                CMMNExpression expression = new CMMNExpression
                {
                    Body = timer.timerExpression.Text.First(),
                    Language = timer.timerExpression.language
                };
                var result = new CaseEltInstance
                {
                    Id = CaseEltInstance.BuildId(casePlanInstanceId, id, 0),
                    EltId = id,
                    NbOccurrence = 0,
                    Name = name, 
                    Type = CasePlanElementInstanceTypes.TIMER
                };
                result.UpdateTimerExpression(expression);
                return result;
            }

            if (planItemDef is tMilestone)
            {
                return new CaseEltInstance
                {
                    Id = CaseEltInstance.BuildId(casePlanInstanceId, id, 0),
                    EltId = id,
                    NbOccurrence = 0,
                    Name = name,
                    Type = CasePlanElementInstanceTypes.MILESTONE
                };
            }

            if (planItemDef is tStage)
            {
                return BuildStage(id, planItemDef as tStage, casePlanInstanceId);
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
            var xmlSerializer = new XmlSerializer(typeof(tStage));
            var strBuilder = new StringBuilder();
            using (var txtWriter = new StringWriter(strBuilder))
            {
                xmlSerializer.Serialize(txtWriter, stage);
            }

            return strBuilder.ToString();
        }
    }
}
