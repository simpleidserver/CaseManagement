using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Handlers
{
    public class CreateCaseInstanceCommandHandler : ICreateCaseInstanceCommandHandler
    {
        private readonly ICMMNDefinitionsQueryRepository _cmmnDefinitionsQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;

        public CreateCaseInstanceCommandHandler(ICMMNDefinitionsQueryRepository cmmnDefinitionsQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository)
        {
            _cmmnDefinitionsQueryRepository = cmmnDefinitionsQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
        }

        public async Task<string> Handle(CreateCaseInstanceCommand createCaseInstanceCommand)
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

            var processFlowInstance = BuildProcessFlowInstance(tCase);
            _processFlowInstanceCommandRepository.Add(processFlowInstance);
            await _processFlowInstanceCommandRepository.SaveChanges();
            return processFlowInstance.Id;
        }

        public static ProcessFlowInstance BuildProcessFlowInstance(tCase tCase)
        {
            var planModel = tCase.casePlanModel;
            var flowInstanceElements = new List<CMMNPlanItem>();
            var connectors = new Dictionary<string, string>();
            foreach(var planItem in planModel.planItem)
            {
                var planItemDef = planModel.Items.First(i => i.id == planItem.definitionRef);
                var flowInstanceElt = CMMNPlanItem.New(planItem.id, planItem.name, BuildPlanItemDefinition(planItemDef));
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

            var builder = ProcessFlowInstanceBuilder.New();
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

        private static CMMNPlanItemDefinition BuildPlanItemDefinition(tPlanItemDefinition planItemDef)
        {
            if (planItemDef is tProcessTask)
            {
                return BuildProcessTask((tProcessTask)planItemDef);
            }

            if (planItemDef is tHumanTask)
            {
                return BuildHumanTask((tHumanTask)planItemDef);
            }

            return null;
        }

        private static CMMNPlanItemDefinition BuildProcessTask(tProcessTask processTask)
        {
            return new CMMNProcessTask(processTask.name);
        }

        private static CMMNPlanItemDefinition BuildHumanTask(tHumanTask humanTask)
        {
            return new CMMNHumanTask(humanTask.name);
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
