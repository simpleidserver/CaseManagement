using CaseManagement.CMMN.Parser;
using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.Common.Expression;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CriteriaResult
    {
        public bool IsSatisfied { get; private set; }
        public bool IsEmpty { get; private set; }
        public Dictionary<string, string> Data { get; private set; }

        public static CriteriaResult Satisfied(Dictionary<string, string> data)
        {
            return new CriteriaResult
            {
                IsSatisfied = true,
                Data = data
            };
        }

        public static CriteriaResult NotSatisifed()
        {
            return new CriteriaResult
            {
                IsSatisfied = false
            };
        }

        public static CriteriaResult Empty()
        {
            return new CriteriaResult
            {
                IsSatisfied = true,
                IsEmpty = true,
                Data = new Dictionary<string, string>()
            };
        }
    }

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

        public string CaseFileId { get; set; }
        public string CasePlanId { get; set; }
        public string NameIdentifier { get; set; }
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
        public ConcurrentBag<BaseCaseEltInstance> Children { get; set; }

        #endregion

        #region Getters

        public ICollection<BaseCasePlanItemInstance> GetNextCasePlanItems(BaseCaseEltInstance elt)
        {
            var lst = new List<BaseCasePlanItemInstance>();
            lst.AddRange(GetFlatListCasePlanItems());
            var children = lst.Where(ch =>
                ch.ExitCriterions.Any(
                    ec => ec.SEntry != null && ec.SEntry.PlanItemOnParts != null && ((ec.SEntry.PlanItemOnParts.Any(pi => pi.SourceRef == elt.EltId)) || (ec.SEntry.FileItemOnParts.Any(pi => pi.SourceRef == elt.EltId)))
                ) ||
                ch.EntryCriterions.Any(
                    ec => ec.SEntry != null && ec.SEntry.PlanItemOnParts != null && ((ec.SEntry.PlanItemOnParts.Any(pi => pi.SourceRef == elt.EltId)) || (ec.SEntry.FileItemOnParts.Any(pi => pi.SourceRef == elt.EltId)))
                )
            ).ToList();
            return children;
        }

        public CriteriaResult IsEntryCriteriaSatisfied(string id)
        {
            var child = GetCasePlanItem(id);
            return IsEntryCriteriaSatisfied(child);
        }

        public CriteriaResult IsEntryCriteriaSatisfied(BaseCasePlanItemInstance elt)
        {
            if (elt.EntryCriterions == null || !elt.EntryCriterions.Any())
            {
                return CriteriaResult.Empty();
            }

            var res = elt.EntryCriterions.Select(_ => IsCriteriaSatisfied(_)).FirstOrDefault(_ => _.IsSatisfied);
            return res == null ? CriteriaResult.NotSatisifed() : res;
        }

        public bool IsRepetitionRuleSatisfied(BaseCasePlanItemInstance elt)
        {
            return elt.RepetitionRule != null && ExpressionParser.IsValid(elt.RepetitionRule.Condition.Body, ExecutionContext);
        }

        public CriteriaResult IsExitCriteriaSatisfied(string id)
        {
            var child = GetCasePlanItem(id);
            return IsExitCriteriaSatisfied(child);
        }

        public CriteriaResult IsExitCriteriaSatisfied(BaseCasePlanItemInstance elt)
        {
            if (elt.ExitCriterions == null || !elt.ExitCriterions.Any())
            {
                return CriteriaResult.NotSatisifed();
            }

            var res = elt.ExitCriterions.Select(c => IsCriteriaSatisfied(c)).FirstOrDefault(c => c.IsSatisfied);
            return res == null ? CriteriaResult.NotSatisifed() : res;
        }

        public CriteriaResult IsCriteriaSatisfied(Criteria criteria)
        {
            var data = new Dictionary<string, string>();
            if (criteria.SEntry.IfPart != null && criteria.SEntry.IfPart.Condition != null)
            {
                if (!ExpressionParser.IsValid(criteria.SEntry.IfPart.Condition, ExecutionContext))
                {
                    return CriteriaResult.NotSatisifed();
                }
            }

            foreach (var planItemOnPart in criteria.SEntry.PlanItemOnParts)
            {
                if (!planItemOnPart.IsConsumed)
                {
                    return CriteriaResult.NotSatisifed();
                }
                
                if (planItemOnPart.IncomingTokens != null)
                {
                    foreach (var kvp in planItemOnPart.IncomingTokens)
                    {
                        data.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            foreach (var caseItemOnPart in criteria.SEntry.FileItemOnParts)
            {
                if (!caseItemOnPart.IsConsumed)
                {
                    return CriteriaResult.NotSatisifed();
                }

                if (caseItemOnPart.IncomingTokens != null)
                {
                    foreach (var kvp in caseItemOnPart.IncomingTokens)
                    {
                        data.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return CriteriaResult.Satisfied(data);
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

        public ICollection<BaseCasePlanItemInstance> GetFlatListCasePlanItems()
        {
            return StageContent.GetFlatListChildren();
        }

        #endregion

        #region Operations

        public void ConsumeTransitionEvts(BaseCaseEltInstance node, string sourceElementId, ICollection<IncomingTransition> transitions)
        {
            var source = GetChild(sourceElementId);
            var target = GetCasePlanItem(node.Id);
            var onParts = new List<IOnPart>();
            onParts.AddRange(GetOnParts(source.EltId, target.EntryCriterions, transitions));
            onParts.AddRange(GetOnParts(source.EltId, target.ExitCriterions, transitions));
            if (!onParts.Any())
            {
                return;
            }

            var evt = new OnPartEvtConsumedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, node.Id, sourceElementId, transitions);
            Handle(evt);
            DomainEvents.Add(evt);
        }

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

        public void MakeTransition(BaseCaseEltInstance element, CMMNTransitions transition, string message = null, Dictionary<string, string> incomingTokens = null, bool isEvtPropagate = true)
        {
            var evt = new CaseElementTransitionRaisedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, element.Id, transition, message, incomingTokens, DateTime.UtcNow);
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

        public bool TryAddWorkerTask(string casePlanInstanceElementId, string externalId)
        {
            var workerTask = WorkerTasks.FirstOrDefault(_ => _.CasePlanElementInstanceId == casePlanInstanceElementId);
            if (workerTask != null)
            {
                return false;
            }

            var evt = new CaseInstanceWorkerTaskAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, casePlanInstanceElementId, externalId, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            return true;
        }

        public bool WorkerTaskExists(string casePlanInstanceElementId)
        {
            return WorkerTasks.Any(_ => _.CasePlanElementInstanceId == casePlanInstanceElementId);
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

        public static CasePlanInstanceAggregate New(string id, CasePlanAggregate caseplan, string nameIdentifier, ICollection<CasePlanInstanceRole> permissions, Dictionary<string, string> parameters)
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
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, nameIdentifier, roles, permissions, json, DateTime.UtcNow, caseplan.CaseFileId, caseplan.AggregateId, caseplan.Name, parameters, files);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static CasePlanInstanceAggregate New(string id, StageElementInstance stage, ICollection<CaseFileItemInstance> caseFiles)
        {
            var result = new CasePlanInstanceAggregate();
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, null, new List<CasePlanInstanceRole>(), new List<CasePlanInstanceRole>(), stage.ToJson(), DateTime.UtcNow, null, null, string.Empty, new Dictionary<string, string>(), caseFiles);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public override object Clone()
        {
            var result = new CasePlanInstanceAggregate
            {
                CasePlanId = CasePlanId,
                CaseFileId = CaseFileId,
                AggregateId = AggregateId,
                CreateDateTime = CreateDateTime,
                Version = Version,
                Name = Name,
                State = State,
                UpdateDateTime = UpdateDateTime,
                NameIdentifier = NameIdentifier,
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
            child.MakeTransition(evt.Transition, evt.Message, DateTime.UtcNow);
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
            CaseFileId = evt.CaseFileId;
            CasePlanId = evt.CasePlanId;
            Name = evt.CasePlanName;
            NameIdentifier = evt.NameIdentifier;
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
                CreateDateTime = evt.CreateDateTime,
                ExternalId = evt.ExternalId
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

        private void Handle(OnPartEvtConsumedEvent evt)
        {
            var source = GetChild(evt.SourceElementId);
            var target = GetCasePlanItem(evt.ElementId);
            Consume(source.EltId, target.EntryCriterions, evt);
            Consume(source.EltId, target.ExitCriterions, evt);
        }

        private void Consume(string sourceEltId, ICollection<Criteria> criterias, OnPartEvtConsumedEvent evt)
        {
            var parts = GetOnParts(sourceEltId, criterias, evt.Transitions);
            foreach(var part in parts)
            {
                var transition = evt.Transitions.FirstOrDefault(tr => tr.Transition == part.StandardEvent);
                part.Consume(transition.IncomingTokens);
            }
        }

        private static ICollection<IOnPart> GetOnParts(string sourceEltId, ICollection<Criteria> criterias, ICollection<IncomingTransition> transitions)
        {
            var result = new List<IOnPart>();
            if (criterias == null)
            {
                return result;
            }

            foreach(var criteria in criterias)
            {
                if (criteria.SEntry == null)
                {
                    continue;
                }

                foreach(var onPart in criteria.SEntry.PlanItemOnParts)
                {
                    if (onPart.IsConsumed)
                    {
                        continue;
                    }

                    if (onPart.SourceRef == sourceEltId)
                    {
                        var transition = transitions.FirstOrDefault(tr => tr.Transition == onPart.StandardEvent);
                        if (transition != null)
                        {
                            result.Add(onPart);
                        }
                    }
                }

                foreach(var onPart in criteria.SEntry.FileItemOnParts)
                {
                    if (onPart.IsConsumed)
                    {
                        continue;
                    }

                    if (onPart.SourceRef == sourceEltId)
                    {
                        var transition = transitions.FirstOrDefault(tr => tr.Transition == onPart.StandardEvent);
                        if (transition != null)
                        {
                            result.Add(onPart);
                        }
                    }
                }
            }

            return result;
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
