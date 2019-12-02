﻿using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public class CreateCaseInstanceCommandHandler : BaseCommandHandler<ProcessFlowInstance>, ICreateCaseInstanceCommandHandler
    {
        private readonly ICMMNDefinitionsQueryRepository _cmmnDefinitionsQueryRepository;

        public CreateCaseInstanceCommandHandler(ICMMNDefinitionsQueryRepository cmmnDefinitionsQueryRepository, IStoreEvents storeEvents, IEventBus eventBus, IAggregateSnapshotStore<ProcessFlowInstance> aggregateSnapshotStore,IOptions<SnapshotConfiguration> snapshotConfiguration) : base(storeEvents, eventBus, aggregateSnapshotStore, snapshotConfiguration)
        {
            _cmmnDefinitionsQueryRepository = cmmnDefinitionsQueryRepository;
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
            await Commit(processFlowInstance, processFlowInstance.GetStreamName());
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

            var builder = ProcessFlowInstanceBuilder.New(id, planModel.name);
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

            if (planItemDef is tTask)
            {
                return BuildTask((tTask)planItemDef);
            }

            return null;
        }

        private static CMMNTask BuildTask(tTask task)
        {
            return new CMMNTask(task.name);
        }

        private static CMMNPlanItemDefinition BuildProcessTask(tProcessTask processTask)
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

        private static CMMNPlanItemDefinition BuildHumanTask(tHumanTask humanTask)
        {
            return new CMMNHumanTask(humanTask.name) { FormId = humanTask.caseFormRef, IsBlocking = humanTask.isBlocking };
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
