using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Parser;
using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using CaseManagement.Common.Expression;
using System;
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
            WorkerTasks = new List<CasePlanInstanceWorkerTask>();
            ExecutionContextVariables = new Dictionary<string, string>();
            Files = new List<CasePlanInstanceFileItem>();
            Children = new List<CaseEltInstance>();
        }

        #region Properties

        public string CaseFileId { get; set; }
        public string CasePlanId { get; set; }
        public string NameIdentifier { get; set; }
        public string Name { get; set; }
        public CaseStates? State { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public CasePlanInstanceExecutionContext ExecutionContext
        {
            get
            {
                return new CasePlanInstanceExecutionContext(this)
                {
                    Variables = ExecutionContextVariables
                };
            }
        }
        public CaseEltInstance StageContent => Children.FirstOrDefault(c => c.Type == CasePlanElementInstanceTypes.STAGE);
        public IReadOnlyCollection<CaseEltInstance> FileItems => Children.Where(c => c.Type == CasePlanElementInstanceTypes.FILEITEM).ToList();
        public Dictionary<string, string> ExecutionContextVariables { get; set; }
        public virtual ICollection<CasePlanInstanceRole> Roles { get; set; }
        public virtual ICollection<CasePlanInstanceFileItem> Files { get; set; }
        public virtual ICollection<CasePlanInstanceWorkerTask> WorkerTasks { get; set; }
        public virtual ICollection<CaseEltInstance> Children { get; set; }

        #endregion

        #region Getters

        public ICollection<CaseEltInstance> GetNextCasePlanItems(CaseEltInstance elt)
        {
            var lst = new List<CaseEltInstance>();
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

        public CriteriaResult IsEntryCriteriaSatisfied(CaseEltInstance elt)
        {
            if (elt.EntryCriterions == null || !elt.EntryCriterions.Any())
            {
                return CriteriaResult.Empty();
            }

            var res = elt.EntryCriterions.Select(_ => IsCriteriaSatisfied(_)).FirstOrDefault(_ => _.IsSatisfied);
            return res == null ? CriteriaResult.NotSatisifed() : res;
        }

        public bool IsRepetitionRuleSatisfied(CaseEltInstance elt)
        {
            return elt.RepetitionRule != null && ExpressionParser.IsValid(elt.RepetitionRule.Condition.Body, ExecutionContext);
        }

        public CriteriaResult IsExitCriteriaSatisfied(string id)
        {
            var child = GetCasePlanItem(id);
            return IsExitCriteriaSatisfied(child);
        }

        public CriteriaResult IsExitCriteriaSatisfied(CaseEltInstance elt)
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

        public CaseEltInstance GetChild(string id)
        {
            var child = Children.FirstOrDefault(c => c.Id == id);
            if (child != null)
            {
                return child;
            }

            return GetCasePlanItem(id);
        }

        public CaseEltInstance GetCasePlanItem(string id)
        {
            if (StageContent.Id == id)
            {
                return StageContent;
            }

            return StageContent.GetChild(id);
        }

        public CaseEltInstance GetCasePlanItemParent(string id)
        {
            return StageContent.GetParent(id);
        }

        public ICollection<CaseEltInstance> GetFlatListCasePlanItems()
        {
            return StageContent.GetFlatListChildren();
        }

        #endregion

        #region Operations

        public void ConsumeTransitionEvts(CaseEltInstance node, string sourceElementId, ICollection<IncomingTransition> transitions)
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

        public void MakeTransition(CaseEltInstance element, CMMNTransitions transition, string message = null, Dictionary<string, string> incomingTokens = null, bool isEvtPropagate = true)
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

        public CaseEltInstance TryCreateInstance(CaseEltInstance elt)
        {
            var evt = new CasePlanItemInstanceCreatedEvent(Guid.NewGuid().ToString(), AggregateId, Version, elt.Id, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            var id = CaseEltInstance.BuildId(AggregateId, elt.EltId, elt.NbOccurrence + 1);
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

        public static CasePlanInstanceAggregate New(string id, CasePlanAggregate caseplan, string nameIdentifier, Dictionary<string, string> parameters)
        {
            var result = new CasePlanInstanceAggregate();
            var roles = caseplan.Roles.Select(_ => new CasePlanInstanceRole
            {
                EltId = _.EltId,
                Name = _.Name
            }).ToList();
            var files = caseplan.Files.Select(_ =>
            {
                var result = new CaseEltInstance
                {
                    EltId = _.EltId,
                    Id = CaseEltInstance.BuildId(id, _.EltId),
                    Name = _.Name
                };
                result.UpdateDefinitionType(_.DefinitionType);
                return result;
            }).ToList();
            var stage = CMMNParser.ExtractStage(caseplan.XmlContent, id);
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, nameIdentifier, roles, stage, DateTime.UtcNow, caseplan.CaseFileId, caseplan.AggregateId, caseplan.Name, parameters, files);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static CasePlanInstanceAggregate New(string id, CaseEltInstance stage, ICollection<CaseEltInstance> caseFiles)
        {
            var result = new CasePlanInstanceAggregate();
            var evt = new CasePlanInstanceCreatedEvent(Guid.NewGuid().ToString(), id, 0, null, new List<CasePlanInstanceRole>(), stage, DateTime.UtcNow, null, null, string.Empty, new Dictionary<string, string>(), caseFiles);
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
                Files = Files.Select(_ => (CasePlanInstanceFileItem)_.Clone()).ToList(),
                Roles = Roles.Select(_ => (CasePlanInstanceRole)_.Clone()).ToList(),
                WorkerTasks = WorkerTasks.Select(_ => (CasePlanInstanceWorkerTask)_.Clone()).ToList(),
                ExecutionContextVariables = ExecutionContextVariables.ToDictionary(c => c.Key, c => c.Value),
                Children = Children.Select(_ => (CaseEltInstance)_.Clone()).ToList()
            };
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
            AggregateId = evt.AggregateId;
            Children.Add(evt.Stage);
            if (evt.Files != null && evt.Files.Any())
            {
                foreach(var file in evt.Files)
                {
                    Children.Add(file);
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
            var role = Roles.FirstOrDefault(_ => _.EltId == evt.RoleId);
            if (role == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"unknown role '{evt.RoleId}'")
                });
            }

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
            parent.AddChild(child.NewOccurrence(AggregateId));
        }

        private void Handle(OnPartEvtConsumedEvent evt)
        {
            var source = GetChild(evt.SourceElementId);
            var target = GetCasePlanItem(evt.ElementId);
            Consume(source.EltId, target.EntryCriterions, evt);
            Consume(source.EltId, target.ExitCriterions, evt);
        }

        private void Consume(string sourceEltId, IReadOnlyCollection<Criteria> criterias, OnPartEvtConsumedEvent evt)
        {
            var parts = GetOnParts(sourceEltId, criterias, evt.Transitions);
            foreach(var part in parts)
            {
                var transition = evt.Transitions.FirstOrDefault(tr => tr.Transition == part.StandardEvent);
                part.Consume(transition.IncomingTokens);
            }
        }

        private static ICollection<IOnPart> GetOnParts(string sourceEltId, IReadOnlyCollection<Criteria> criterias, ICollection<IncomingTransition> transitions)
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
        private void PropagateTransition(CaseEltInstance elt, CMMNTransitions transition)
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

            if (elt.Type != CasePlanElementInstanceTypes.STAGE)
            {
                return;
            }

            foreach(var child in elt.Children)
            {
                MakeTransition(child, newTransition.Value);
            }
        }
    }
}
