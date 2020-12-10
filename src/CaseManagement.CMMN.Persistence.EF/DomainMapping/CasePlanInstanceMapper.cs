using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Persistence.EF.DomainMapping
{
    public static class CasePlanInstanceMapper
    {
        #region Domain

        public static CasePlanInstanceAggregate ToDomain(this CasePlanInstanceModel record)
        {
            return new CasePlanInstanceAggregate
            {
                AggregateId = record.Id,
                Version = record.Version,
                CaseFileId = record.CaseFileId,
                CasePlanId = record.CasePlanId,
                NameIdentifier = record.NameIdentifier,
                Name = record.Name,
                State = record.CaseState == null ? (CaseStates?)null : (CaseStates)record.CaseState,
                ExecutionContext = string.IsNullOrWhiteSpace(record.ExecutionContext) ? null : JsonConvert.DeserializeObject<CasePlanInstanceExecutionContext>(record.ExecutionContext),
                Roles = record.Roles.Select(_ => ToCasePlanInstanceRoleDomain(_)).ToList(),
                Files = new ConcurrentBag<CasePlanInstanceFileItem>(record.Files.Select(_ => ToDomain(_)).ToList()),
                WorkerTasks = new ConcurrentBag<CasePlanInstanceWorkerTask>(record.WorkerTasks.Select(_ => ToDomain(_)).ToArray()),
                Children = new ConcurrentBag<BaseCaseEltInstance>(record.Children.Where(_ => _.ParentId == null).Select(c => c.ToElementInstance()).ToList()),
                CreateDateTime = record.CreateDateTime,
                UpdateDateTime = record.UpdateDateTime
            };
        }

        public static BaseCaseEltInstance ToElementInstance(this CasePlanElementInstanceModel model)
        {
            var type = (CasePlanElementInstanceTypes)model.Type;
            switch(type)
            {
                case CasePlanElementInstanceTypes.HUMANTASK:
                    return JsonConvert.DeserializeObject<HumanTaskElementInstance>(model.SerializedContent);
                case CasePlanElementInstanceTypes.STAGE:
                    var stage = JsonConvert.DeserializeObject<StageElementInstance>(model.SerializedContent);
                    if(model.Children != null && model.Children.Any())
                    {
                        foreach(var child in model.Children)
                        {
                            stage.Children.Add(child.ToElementInstance() as BaseCasePlanItemInstance);
                        }
                    }

                    return stage;
                case CasePlanElementInstanceTypes.FILEITEM:
                    return JsonConvert.DeserializeObject<CaseFileItemInstance>(model.SerializedContent);
                case CasePlanElementInstanceTypes.EMPTYTASK:
                    return JsonConvert.DeserializeObject<EmptyTaskElementInstance>(model.SerializedContent);
                case CasePlanElementInstanceTypes.MILESTONE:
                    return JsonConvert.DeserializeObject<MilestoneElementInstance>(model.SerializedContent);
                case CasePlanElementInstanceTypes.TIMER:
                    return JsonConvert.DeserializeObject<TimerEventListener>(model.SerializedContent);
            }

            return null;
        }

        public static CasePlanInstanceRole ToCasePlanInstanceRoleDomain(this RoleModel record)
        {
            return new CasePlanInstanceRole
            {
                Id = record.RoleId,
                Name = record.RoleName,
                Claims = record.Claims.Select(_ => new KeyValuePair<string, string>(_.ClaimName, _.ClaimValue)).ToList(),
            };
        }

        public static CasePlanInstanceFileItem ToDomain(this CasePlanInstanceFileItemModel record)
        {
            return new CasePlanInstanceFileItem
            {
                CaseFileItemType = record.CaseFileItemType,
                CasePlanElementInstanceId = record.CasePlanElementInstanceId,
                ExternalValue = record.ExternalValue
            };
        }

        public static CasePlanInstanceWorkerTask ToDomain(this CasePlanInstanceWorkerTaskModel caseWorkerTask)
        {
            return new CasePlanInstanceWorkerTask
            {
                CasePlanElementInstanceId = caseWorkerTask.CasePlanElementInstanceId,
                CreateDateTime = caseWorkerTask.CreateDateTime
            };
        }

        #endregion

        #region Model

        public static CasePlanInstanceModel ToModel(this CasePlanInstanceAggregate record)
        {
            var result = new CasePlanInstanceModel
            {
                Id = record.AggregateId,
                Version = record.Version,
                CaseFileId = record.CaseFileId,
                CasePlanId = record.CasePlanId,
                Name = record.Name,
                NameIdentifier = record.NameIdentifier,
                CaseState = record.State == null ? null : (int?)record.State,
                ExecutionContext = record.ExecutionContext == null ? null : JsonConvert.SerializeObject(record.ExecutionContext),
                Roles = record.Roles.Select(_ => ToModel(_, record.AggregateId)).ToList(),
                Files = record.Files.Select(_ => ToModel(_, record.AggregateId)).ToList(),
                WorkerTasks = record.WorkerTasks.Select(_ => ToModel(_, record.AggregateId)).ToList(),
                Children =  record.Children == null ? new List<CasePlanElementInstanceModel>() : record.Children.Select(_ => _.ToModel(record.AggregateId)).ToList(),
                CreateDateTime = record.CreateDateTime,
                UpdateDateTime = record.UpdateDateTime
            };
            return result;
        }

        public static CasePlanElementInstanceModel ToModel(this StageElementInstance stage, string casePlanInstanceId)
        {
            var children = new List<CasePlanElementInstanceModel>();
            foreach(var child in stage.Children)
            {
                children.Add(child.ToModel(casePlanInstanceId));
            }

            stage.Children.Clear();
            return new CasePlanElementInstanceModel
            {
                EltId = stage.Id,
                CasePlanInstanceId = casePlanInstanceId,
                SerializedContent = JsonConvert.SerializeObject(stage),
                Type = (int)stage.Type,
                Children = children
            };
        }

        public static CasePlanElementInstanceModel ToModel(this BaseCaseEltInstance casePlanElementInstance, string casePlanInstanceId)
        {
            var stage = casePlanElementInstance as StageElementInstance;
            if (stage != null)
            {
                return stage.ToModel(casePlanInstanceId);
            }

            return new CasePlanElementInstanceModel
            {
                CasePlanInstanceId = casePlanInstanceId,
                EltId = casePlanElementInstance.Id,
                Type = (int)casePlanElementInstance.Type,
                SerializedContent = JsonConvert.SerializeObject(casePlanElementInstance)
            };
        }

        public static RoleModel ToModel(this CasePlanInstanceRole role, string casePlanInstanceId)
        {
            return new RoleModel
            {
                RoleId = role.Id,
                IsCaseOwner = false,
                RoleName = role.Name,
                Claims = role.Claims.Select(_ => new RoleClaimModel
                {
                    ClaimName = _.Key,
                    ClaimValue = _.Value
                }).ToList(),
                CasePlanInstanceId = casePlanInstanceId,
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow
            };
        }

        public static CasePlanInstanceFileItemModel ToModel(this CasePlanInstanceFileItem file, string casePlanInstanceId)
        {
            return new CasePlanInstanceFileItemModel
            {
                CaseFileItemType = file.CaseFileItemType,
                CasePlanElementInstanceId = file.CasePlanElementInstanceId,
                ExternalValue = file.ExternalValue,
                CasePlanInstanceId = casePlanInstanceId
            };
        }

        public static CasePlanInstanceWorkerTaskModel ToModel(this CasePlanInstanceWorkerTask workerTask, string casePlanInstanceId)
        {
            return new CasePlanInstanceWorkerTaskModel
            {
                CasePlanElementInstanceId = workerTask.CasePlanElementInstanceId,
                CasePlanInstanceId = casePlanInstanceId,
                CreateDateTime = workerTask.CreateDateTime
            };
        }

        #endregion
    }
}
