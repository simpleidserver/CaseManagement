using CaseManagement.CMMN.Common.Exceptions;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Infrastructure;
using CaseManagement.CMMN.Parser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceAggregate : BaseAggregate
    {
        public CasePlanInstanceAggregate()
        {
            Roles = new List<CasePlanInstanceRole>();
            WorkerTasks = new ConcurrentBag<CasePlanInstanceWorkerTask>();
            ExecutionContext = new CasePlanInstanceExecutionContext(this);
            Files = new ConcurrentBag<CasePlanInstanceFileItem>();
            Children = new ConcurrentBag<BaseCaseEltInstance>();
        }

        #region Properties

        public string CasePlanId { get; set; }
        public string CaseOwner { get; set; }
        public string Name { get; set; }
        public CaseStates? State { get; set; }
        public CasePlanInstanceExecutionContext ExecutionContext { get; set; }
        public ICollection<CasePlanInstanceRole> Roles { get; set; }
        public ConcurrentBag<CasePlanInstanceFileItem> Files { get; set; }
        public ConcurrentBag<CasePlanInstanceWorkerTask> WorkerTasks { get; set; }
        public StageElementInstance StageContent => (StageElementInstance)Children.FirstOrDefault(c => c is StageElementInstance);
        public ICollection<CaseFileItemInstance> FileItems => Children.Where(c => c is CaseFileItemInstance).Cast<CaseFileItemInstance>().ToList();
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public CasePlanInstanceRole CaseOwnerRole => string.IsNullOrWhiteSpace(CaseOwner) ? null : Roles.First(_ => _.Id == CaseOwner);
        public ConcurrentBag<BaseCaseEltInstance> Children { get; set; }

        #endregion

        #region Getters

        public bool IsEntryCriteriaSatisfied(string id)
        {
            var child = GetCasePlanItem(id);
            return IsEntryCriteriaSatisfied(child);
        }

        public bool IsEntryCriteriaSatisfied(BaseCasePlanItemInstance elt)
        {
            return elt.EntryCriterions == null || !elt.EntryCriterions.Any() || elt.EntryCriterions.Any(_ => IsCriteriaSatisfied(_, elt.NbOccurrence));
        }

        public bool IsRepetitionRuleSatisfied(BaseCasePlanItemInstance elt)
        {
            return elt.RepetitionRule != null && ExpressionParser.IsValid(elt.RepetitionRule.Condition.Body, ExecutionContext);
        }

        public bool IsExitCriteriaSatisfied(string id)
        {
            var child = GetCasePlanItem(id);
            return IsExitCriteriaSatisfied(child);
        }

        public bool IsExitCriteriaSatisfied(BaseCasePlanItemInstance elt)
        {
            return elt.ExitCriterions != null && elt.ExitCriterions.Any() && elt.ExitCriterions.Any(_ => IsCriteriaSatisfied(_, elt.NbOccurrence));
        }

        public bool IsCriteriaSatisfied(Criteria criteria, int nbOccurrence)
        {
            if (criteria.SEntry.IfPart != null && criteria.SEntry.IfPart.Condition != null)
            {
                if (!ExpressionParser.IsValid(criteria.SEntry.IfPart.Condition, ExecutionContext))
                {
                    return false;
                }
            }

            foreach (var planItemOnPart in criteria.SEntry.PlanItemOnParts)
            {
                var id = BaseCasePlanItemInstance.BuildId(AggregateId, planItemOnPart.SourceRef, nbOccurrence);
                var source = GetCasePlanItem(id);
                if (planItemOnPart.StandardEvent != source.LatestTransition)
                {
                    return false;
                }
            }

            foreach (var caseItemOnPart in criteria.SEntry.FileItemOnParts)
            {
                var id = CaseFileItemInstance.BuildId(AggregateId, caseItemOnPart.SourceRef);
                var source = GetCaseFileItem(id);
                if (caseItemOnPart.StandardEvent != source.LatestTransition)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsFileExists(string casePlanInstanceElementId)
        {
            return Files.Any(_ => _.CasePlanElementInstanceId == casePlanInstanceElementId);
        }

        public BaseCaseEltInstance GetChild(string id)
        {
            var child = Children.FirstOrDefault(c => c.Id == id);
            if (child != null)
            {
                return child;
            }

            return GetCasePlanItem(id);
        }

        public BaseCasePlanItemInstance GetCasePlanItem(string id)
        {
            if (StageContent.Id == id)
            {
                return StageContent;
            }

            return StageContent.GetChild(id);
        }

        public StageElementInstance GetCasePlanItemParent(string id)
        {
            return StageContent.GetParent(id);
        }

        public CaseFileItemInstance GetCaseFileItem(string id)
        {
            return FileItems.FirstOrDefault(f => f.Id == id);
        }

        public ICollection<BaseCasePlanItemInstance> GetFlatListCasePlanItems()
        {
            return StageContent.GetFlatListChildren();
        }

        #endregion

        #region Operations

        public void MakeTransition(CMMNTransitions transition, bool isEvtPropagate = true)
        {
            var evt = new CaseTransitionRaisedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, transition, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            if (isEvtPropagate)
            {
                PropagateTransition(StageContent, transition);
            }
        }

        public void MakeTransition(BaseCaseEltInstance element, CMMNTransitions transition, bool isEvtPropagate = true)
        {
            var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, element.Id, transition, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            var caseWorkerTask = WorkerTasks.FirstOrDefault(_ => _.CasePlanElementInstanceId == element.Id);
            if (caseWorkerTask != null && (transition == CMMNTransitions.Complete ||
                transition == CMMNTransitions.Terminate ||
                transition == CMMNTransitions.ParentTerminate))
            {
                var removeCWT = new CaseInstanceWorkerTaskRemovedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, element.Id, DateTime.UtcNow);
                Handle(removeCWT);
                DomainEvents.Add(removeCWT);
            }

            if (isEvtPropagate)
            {
                PropagateTransition(element, transition);
            }
        }

        public void UpdatePermission(string roleId, ICollection<KeyValuePair<string, string>> claims)
        {
            var evt = new CaseInstanceRoleUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, roleId, claims, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public bool TryAddWorkerTask(string casePlanInstanceElementId)
        {
            var workerTask = WorkerTasks.FirstOrDefault(_ => _.CasePlanElementInstanceId == casePlanInstanceElementId);
            if (workerTask != null)
            {
                return false;
            }

            var evt = new CaseInstanceWorkerTaskAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, casePlanInstanceElementId, DateTime.UtcNow, CaseOwnerRole);
            Handle(evt);
            DomainEvents.Add(evt);
            return true;
        }

        public bool TryAddCaseFileItem(string casePlanInstanceElementId, string type, string externalValue)
        {
            var file = Files.FirstOrDefault(_ => _.CasePlanElementInstanceId == casePlanInstanceElementId);
            if (file != null)
            {
                return false;
            }

            var evt = new CaseFileItemAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, casePlanInstanceElementId, type, externalValue);
            Handle(evt);
            DomainEvents.Add(evt);
            return true;
        }

        public void SetVariable(string key, string value)
        {
            var evt = new VariableUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, key, value, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public BaseCasePlanItemInstance TryCreateInstance(BaseCasePlanItemInstance elt)
        {
            var evt = new CasePlanItemInstanceCreatedEvent(Guid.NewGuid().ToString(), AggregateId, Version, elt.Id, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            var id = BaseCasePlanItemInstance.BuildId(AggregateId, elt.EltId, elt.NbOccurrence + 1);
            return GetCasePlanItem(id);
        }

        #endregion

        public static CasePlanInstanceAggregate New(ICollection<DomainEvent> evts)
        {
            var result = new CasePlanInstanceAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static CasePlanInstanceAggregate New(string id, CasePlanAggregate caseplan, ICollection<CasePlanInstanceRole> permissions, Dictionary<string, string> parameters)
        {
            var result = new CasePlanInstanceAggregate();
            var roles = caseplan.Roles.Select(_ => new CasePlanInstanceRole
            {
                Id = _.Id,
                Name = _.Name
            }).ToList();
            var files = caseplan.Files.Select(_ => new CaseFileItemInstance
            {
                DefinitionType = _.DefinitionType,
                EltId = _.Id,
                Id = CaseFileItemInstance.BuildId(id, _.Id),
                Name = _.Name
            }).ToList();
            var stage = CMMNParser.ExtractStage(caseplan.XmlContent, id);
            var json = stage.ToJson();
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, caseplan.CaseOwner, roles, permissions, json, DateTime.UtcNow, caseplan.AggregateId, caseplan.Name, parameters, files);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static CasePlanInstanceAggregate New(string id, StageElementInstance stage, ICollection<CaseFileItemInstance> caseFiles)
        {
            var result = new CasePlanInstanceAggregate();
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, null, new List<CasePlanInstanceRole>(), new List<CasePlanInstanceRole>(), stage.ToJson(), DateTime.UtcNow, null, string.Empty, new Dictionary<string, string>(), caseFiles);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public override object Clone()
        {
            var result = new CasePlanInstanceAggregate
            {
                CasePlanId = CasePlanId,
                AggregateId = AggregateId,
                CreateDateTime = CreateDateTime,
                Version = Version,
                Name = Name,
                State = State,
                UpdateDateTime = UpdateDateTime,
                CaseOwner = CaseOwner,
                Files = new ConcurrentBag<CasePlanInstanceFileItem>(Files.Select(_ => (CasePlanInstanceFileItem)_.Clone()).ToList()),
                Roles = Roles.Select(_ => (CasePlanInstanceRole)_.Clone()).ToList(),
                WorkerTasks = new ConcurrentBag<CasePlanInstanceWorkerTask>(WorkerTasks.Select(_ => (CasePlanInstanceWorkerTask)_.Clone()).ToArray()),
                ExecutionContext = (CasePlanInstanceExecutionContext)ExecutionContext.Clone(),
                Children = new ConcurrentBag<BaseCaseEltInstance>(Children.Select(_ => (BaseCaseEltInstance)_.Clone()))
            };
            result.ExecutionContext.CasePlanInstance = result;
            return result;
        }

        public string GetStreamName()
        {
            return GetStreamName(AggregateId);
        }

        public static string GetStreamName(string id)
        {
            return $"case-planinstance-{id}";
        }

        #region Handle domain events

        public override void Handle(dynamic obj)
        {
            Handle(obj);
        }

        private void Handle(CaseElementTransitionRaisedEvent evt)
        {
            var child = GetChild(evt.ElementId);
            child.MakeTransition(evt.Transition, DateTime.UtcNow);
        }

        private void Handle(CasePlanInstanceCreatedEvent evt)
        {
            Roles = evt.Roles.ToList();
            var unknownRoles = evt.Permissions.Where(_ => !Roles.Any(__ => __.Id == _.Id));
            if (unknownRoles.Any())
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"unknown roles '{string.Join(",", unknownRoles.Select(_ => _.Id))}'")
                });
            }

            if (!string.IsNullOrWhiteSpace(evt.CaseOwner))
            {
                if (Roles.FirstOrDefault(_ => _.Id == evt.CaseOwner) == null)
                {
                    throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "validation",  $"unknown case owner role '{evt.CaseOwner}'")
                    });
                }
            }

            foreach(var permission in evt.Permissions)
            {
                var role = Roles.First(_ => _.Id == permission.Id);
                role.Claims = permission.Claims;
            }

            AggregateId = evt.AggregateId;
            if (!string.IsNullOrWhiteSpace(evt.JsonContent))
            {
                Children.Add(StageElementInstance.FromJson(evt.JsonContent));
            }

            if (evt.Files != null && evt.Files.Any())
            {
                foreach(var file in evt.Files)
                {
                    Children.Add(file.Clone() as CaseFileItemInstance);
                }
            }

            CreateDateTime = evt.CreateDateTime;
            UpdateDateTime = evt.CreateDateTime;
            Version = evt.Version;
            CasePlanId = evt.CasePlanId;
            CaseOwner = evt.CaseOwner;
            if (evt.Parameters != null)
            {
                foreach (var kvp in evt.Parameters)
                {
                    ExecutionContext.SetStrVariable(kvp.Key, kvp.Value);
                }
            }
        }

        private void Handle(CaseTransitionRaisedEvent evt)
        {
            switch (evt.Transition)
            {
                case CMMNTransitions.Create:
                    if (State != null)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is already initialized")
                        });
                    }

                    State = CaseStates.Active;
                    break;
                case CMMNTransitions.Complete:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not active")
                        });
                    }

                    State = CaseStates.Completed;
                    break;
                case CMMNTransitions.Terminate:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not active")
                        });
                    }

                    State = CaseStates.Terminated;
                    break;
                case CMMNTransitions.Fault:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not active")
                        });
                    }

                    State = CaseStates.Failed;
                    break;
                case CMMNTransitions.Suspend:
                    if (State != CaseStates.Active)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not active")
                        });
                    }

                    State = CaseStates.Suspended;
                    break;
                case CMMNTransitions.Resume:
                    if (State != CaseStates.Suspended)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not suspended")
                        });
                    }

                    State = CaseStates.Active;
                    break;
                case CMMNTransitions.Close:
                    if (State != CaseStates.Completed &&
                        State != CaseStates.Terminated &&
                        State != CaseStates.Failed &&
                        State != CaseStates.Suspended)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not completed / terminated / failed / suspended")
                        });
                    }

                    State = CaseStates.Closed;
                    break;
                case CMMNTransitions.Reactivate:
                    if (State != CaseStates.Completed &&
                        State != CaseStates.Terminated &&
                        State != CaseStates.Failed &&
                        State != CaseStates.Suspended)
                    {
                        throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "transition",  "case instance is not completed / terminated / failed / suspended")
                        });
                    }

                    State = CaseStates.Active;
                    break;
            }

            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(CaseInstanceRoleUpdatedEvent evt)
        {
            var role = Roles.FirstOrDefault(_ => _.Id == evt.RoleId);
            if (role == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"unknown role '{evt.RoleId}'")
                });
            }

            role.Claims = evt.Claims;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(CaseInstanceWorkerTaskAddedEvent evt)
        {
            var child = GetCasePlanItem(evt.CasePlanInstanceElementId);
            if (child == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"unknown child '{evt.CasePlanInstanceElementId}'")
                });
            }

            WorkerTasks.Add(new CasePlanInstanceWorkerTask
            {
                CasePlanElementInstanceId = evt.CasePlanInstanceElementId,
                CreateDateTime = evt.CreateDateTime
            });
            UpdateDateTime = evt.CreateDateTime;
            Version = evt.Version;
        }

        private void Handle(CaseInstanceWorkerTaskRemovedEvent evt)
        {
            var workertask = WorkerTasks.FirstOrDefault(_ => _.CasePlanElementInstanceId == evt.CasePlanInstanceElementId);
            if (workertask == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"case worker task doesn't exist '{workertask.CasePlanElementInstanceId}'")
                });
            }

            WorkerTasks.Remove(workertask);
            Version = evt.Version;
            UpdateDateTime = evt.ExecutionDateTime;
        }

        private void Handle(CaseFileItemAddedEvent evt)
        {
            var file = Files.FirstOrDefault(_ => _.CasePlanElementInstanceId == evt.CasePlanInstanceElementId);
            if (file != null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"case file item '{evt.CasePlanInstanceElementId}' already exists")
                });
            }

            Files.Add(new CasePlanInstanceFileItem
            {
                CaseFileItemType = evt.Type,
                CasePlanElementInstanceId = evt.CasePlanInstanceElementId,
                ExternalValue = evt.ExternalValue
            });
        }

        private void Handle(VariableUpdatedEvent evt)
        {
            ExecutionContext.UpdateStrVariable(evt.Key, evt.Value);
            Version = evt.Version;
            UpdateDateTime = evt.UpdateDateTime;
        }

        private void Handle(CasePlanItemInstanceCreatedEvent evt)
        {
            var parent = GetCasePlanItemParent(evt.CasePlanItemInstanceId);
            if (parent == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"case plan item '{evt.CasePlanItemInstanceId}' doesn't exist")
                });
            }

            var child = GetCasePlanItem(evt.CasePlanItemInstanceId);
            parent.Children.Add(child.NewOccurrence(AggregateId));
        }

        #endregion

        /// <summary>
        /// Propagate transition to children.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="transition"></param>
        private void PropagateTransition(BaseCaseEltInstance elt, CMMNTransitions transition)
        {
            CMMNTransitions? newTransition = null;
            if (transition == CMMNTransitions.Terminate || transition == CMMNTransitions.ParentTerminate)
            {
                newTransition = CMMNTransitions.ParentTerminate;
            }

            if (newTransition == null)
            {
                return;
            }

            var stage = elt as StageElementInstance;
            if (stage == null)
            {
                return;
            }

            foreach(var child in stage.Children)
            {
                MakeTransition(child, newTransition.Value);
            }
        }
    }
}
