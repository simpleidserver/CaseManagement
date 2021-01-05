using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.CasePlanInstance.Results
{
    public class CasePlanInstanceResult
    {
        public CasePlanInstanceResult()
        {
            ExecutionContext = new Dictionary<string, string>();
            Files = new List<CasePlanInstanceFileItemResult>();
            Roles = new List<CasePlanInstanceRoleResult>();
            Children = new List<CasePlanItemInstanceResult>();
            WorkerTasks = new List<WorkerTaskResult>();
        }

        public string Id { get; set; }
        public string CaseFileId { get; set; }
        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public Dictionary<string, string> ExecutionContext { get; set; }
        public ICollection<CasePlanInstanceFileItemResult> Files { get; set; }
        public ICollection<CasePlanInstanceRoleResult> Roles { get; set; }
        public ICollection<CasePlanItemInstanceResult> Children { get; set; }
        public ICollection<WorkerTaskResult> WorkerTasks { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public class CasePlanInstanceFileItemResult
        {
            public string CasePlanElementInstanceId { get; set; }
            public string CaseFileItemType { get; set; }
            public string ExternalValue { get; set; }

            public static CasePlanInstanceFileItemResult ToDTO(CasePlanInstanceFileItem result)
            {
                return new CasePlanInstanceFileItemResult
                {
                    CaseFileItemType = result.CaseFileItemType,
                    CasePlanElementInstanceId = result.CasePlanElementInstanceId,
                    ExternalValue = result.ExternalValue
                };
            }
        }

        public class WorkerTaskResult
        {
            public string CasePlanElementInstanceId { get; set; }
            public string ExternalId { get; set; }
            public DateTime CreateDateTime { get; set; }

            public static WorkerTaskResult ToDTO(CasePlanInstanceWorkerTask workerTask)
            {
                return new WorkerTaskResult
                {
                    CasePlanElementInstanceId = workerTask.CasePlanElementInstanceId,
                    CreateDateTime = workerTask.CreateDateTime,
                    ExternalId = workerTask.ExternalId
                };
            }
        }


        public class CasePlanInstanceRoleResult
        {
            public string Id { get; set; }
            public string Name { get; set; }
            
            public static CasePlanInstanceRoleResult ToDto(CasePlanInstanceRole role)
            {
                return new CasePlanInstanceRoleResult
                {
                    Id = role.Id,
                    Name = role.Name
                };
            }
        }

        public class CasePlanItemInstanceResult
        {
            public string Id { get; set; }
            public string EltId { get; set; }
            public string ParentEltId { get; set; }
            public int NbOccurrence { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string State { get; set; }
            public string FormId { get; set; }
            public ICollection<TransitionHistoryResult> TransitionHistories { get; set; }

            public static CasePlanItemInstanceResult ToDto(BaseCasePlanItemInstance casePlanItemInstance)
            {
                string stateStr = null, formId = null;
                if (casePlanItemInstance is BaseTaskOrStageElementInstance)
                {
                    var state = ((BaseTaskOrStageElementInstance)casePlanItemInstance).State;
                    stateStr = state == null ? null : Enum.GetName(typeof(TaskStageStates), state);
                }

                if (casePlanItemInstance is BaseMilestoneOrTimerElementInstance)
                {
                    var state = ((BaseMilestoneOrTimerElementInstance)casePlanItemInstance).State;
                    stateStr = state == null ? null : Enum.GetName(typeof(MilestoneEventStates), state);
                }

                if (casePlanItemInstance is HumanTaskElementInstance)
                {
                    var humanTaskInstance = casePlanItemInstance as HumanTaskElementInstance;
                    formId = humanTaskInstance.FormId;
                }

                return new CasePlanItemInstanceResult
                {
                    Id = casePlanItemInstance.Id,
                    Name = casePlanItemInstance.Name,
                    NbOccurrence = casePlanItemInstance.NbOccurrence,
                    EltId = casePlanItemInstance.EltId,
                    State = stateStr,
                    FormId = formId,
                    Type = Enum.GetName(typeof(CasePlanElementInstanceTypes), casePlanItemInstance.Type).ToUpperInvariant(),
                    TransitionHistories = casePlanItemInstance.TransitionHistories.Select(_ => TransitionHistoryResult.ToDto(_)).ToList()
                };
            }
        }

        public class TransitionHistoryResult
        {
            public string Transition { get; set; }
            public DateTime ExecutionDateTime { get; set; }
            public string Message { get; set; }

            public static TransitionHistoryResult ToDto(CasePlanElementInstanceTransitionHistory history)
            {
                return new TransitionHistoryResult
                {
                    ExecutionDateTime = history.ExecutionDateTime,
                    Message = history.Message,
                    Transition = Enum.GetName(typeof(CMMNTransitions), history.Transition)
                };
            }
        }

        public static CasePlanInstanceResult ToDto(CasePlanInstanceAggregate casePlanInstance)
        {
            var children = casePlanInstance.GetFlatListCasePlanItems().Select(_ => CasePlanItemInstanceResult.ToDto(_)).ToList();
            foreach(var elt in casePlanInstance.Children.Where(_ => _ is StageElementInstance))
            {
                var stage = elt as StageElementInstance;
                foreach(var childEltId in stage.Children.Select(_ => _.EltId))
                {
                    foreach(var child in children.Where(_ => _.EltId == childEltId))
                    {
                        child.ParentEltId = elt.EltId;
                    }
                }
            }

            return new CasePlanInstanceResult
            {
                Id = casePlanInstance.AggregateId,
                CaseFileId = casePlanInstance.CaseFileId,
                CasePlanId = casePlanInstance.CasePlanId,
                Name = casePlanInstance.Name,
                State = casePlanInstance.State == null ? string.Empty : Enum.GetName(typeof(CaseStates), casePlanInstance.State),
                Roles = casePlanInstance.Roles.Select(_ => CasePlanInstanceRoleResult.ToDto(_)).ToList(),
                CreateDateTime = casePlanInstance.CreateDateTime,
                UpdateDateTime = casePlanInstance.UpdateDateTime,
                Children = children,
                Files = casePlanInstance.Files.Select(_ => CasePlanInstanceFileItemResult.ToDTO(_)).ToList(),
                WorkerTasks = casePlanInstance.WorkerTasks.Select(_ => WorkerTaskResult.ToDTO(_)).ToList(),
                ExecutionContext = casePlanInstance.ExecutionContext == null ? new Dictionary<string, string>() : casePlanInstance.ExecutionContext.Variables.ToDictionary(k => k.Key, k => k.Value)
            };
        }
    }
}
