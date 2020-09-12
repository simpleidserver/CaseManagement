using CaseManagement.CMMN.Common.Exceptions;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Infrastructure;
using CaseManagement.CMMN.Parser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            ExecutionContext = new CasePlanInstanceExecutionContext();
            Files = new ConcurrentBag<CasePlanInstanceFileItem>();
        }

        public string CasePlanId { get; set; }
        public string CaseOwner { get; set; }
        public string Name { get; set; }
        public CaseStates? State { get; set; }
        public CasePlanInstanceExecutionContext ExecutionContext { get; set; }
        public ICollection<CasePlanInstanceRole> Roles { get; set; }
        public ConcurrentBag<CasePlanInstanceFileItem> Files { get; set; }
        public ConcurrentBag<CasePlanInstanceWorkerTask> WorkerTasks { get; set; }
        public StageElementInstance Content { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public CasePlanInstanceRole CaseOwnerRole => string.IsNullOrWhiteSpace(CaseOwner) ? null : Roles.First(_ => _.Id == CaseOwner);

        public bool IsEntryCriteriaSatisfied(string id)
        {
            var child = GetChild(id);
            return IsEntryCriteriaSatisfied(child);
        }

        public bool IsEntryCriteriaSatisfied(CasePlanElementInstance elt)
        {
            return elt.EntryCriterions == null || !elt.EntryCriterions.Any() || elt.EntryCriterions.Any(_ => IsCriteriaSatisfied(_));
        }

        public bool IsExitCriteriaSatisfied(string id)
        {
            var child = GetChild(id);
            return IsExitCriteriaSatisfied(child);
        }

        public bool IsExitCriteriaSatisfied(CasePlanElementInstance elt)
        {
            return elt.ExitCriterions != null && elt.ExitCriterions.Any() && elt.ExitCriterions.Any(_ => IsCriteriaSatisfied(_));
        }

        public bool IsCriteriaSatisfied(Criteria criteria)
        {
            Func<string, CMMNTransitions, bool> callback = (sourceRef, standardEvent) =>
            {
                var source = GetChild(sourceRef);
                if (standardEvent != source.LatestTransition)
                {
                    return false;
                }

                return true;
            };
            foreach (var planItemOnPart in criteria.SEntry.PlanItemOnParts)
            {
                if (!callback(planItemOnPart.SourceRef, planItemOnPart.StandardEvent))
                {
                    return false;
                }
            }

            foreach (var caseItemOnPart in criteria.SEntry.FileItemOnParts)
            {
                if (!callback(caseItemOnPart.SourceRef, caseItemOnPart.StandardEvent))
                {
                    return false;
                }
            }

            if (criteria.SEntry.IfPart != null && criteria.SEntry.IfPart.Condition != null)
            {
                if (!ExpressionParser.IsValid(criteria.SEntry.IfPart.Condition, ExecutionContext))
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

        public CasePlanElementInstance GetChild(string id)
        {
            if (Content.Id == id)
            {
                return Content;
            }

            return Content.GetChild(id);
        }

        public ICollection<CasePlanElementInstance> GetFlatListChildren()
        {
            return Content.GetFlatListChildren();
        }

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
            var stage = CMMNParser.ExtractStage(caseplan.XmlContent, id);
            var json = stage.ToJson();
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, caseplan.CaseOwner, roles, permissions, json, DateTime.UtcNow, caseplan.AggregateId, parameters, caseplan.Files);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static CasePlanInstanceAggregate New(string id, StageElementInstance stage)
        {
            var result = new CasePlanInstanceAggregate();
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, null, new List<CasePlanInstanceRole>(), new List<CasePlanInstanceRole>(), stage.ToJson(), DateTime.UtcNow, null, new Dictionary<string, string>(), new List<CasePlanFileItem>());
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        #region Operations

        public void MakeTransition(CMMNTransitions transition, bool isEvtPropagate = true)
        {
            var evt = new CaseTransitionRaisedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, transition, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            if (isEvtPropagate)
            {
                PropagateTransition(Content, transition);
            }
        }

        public void MakeTransition(CasePlanElementInstance element, CMMNTransitions transition, bool isEvtPropagate = true)
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

        #endregion

        public override object Clone()
        {
            return new CasePlanInstanceAggregate
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
                Content = (StageElementInstance)Content.Clone(),
                ExecutionContext = (CasePlanInstanceExecutionContext)ExecutionContext.Clone()
            };
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
            Content = StageElementInstance.FromJson(evt.JsonContent);
            if (evt.Files != null && evt.Files.Any())
            {
                foreach(var file in evt.Files)
                {
                    Content.AddChild(new CaseFileItemInstance
                    {
                        DefinitionType = file.DefinitionType,
                        Id = file.Id,
                        Name = file.Name
                    });
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
                    ExecutionContext.SetVariable(kvp.Key, kvp.Value);
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
            var child = GetChild(evt.CasePlanInstanceElementId);
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

        #endregion

        /// <summary>
        /// Propagate transition to children.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="transition"></param>
        private void PropagateTransition(CasePlanElementInstance elt, CMMNTransitions transition)
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
