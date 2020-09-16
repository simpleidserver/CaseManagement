using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Persistence.EF.DomainMapping
{
    public static class CaseWorkerTaskMapper
    {
        #region To domain

        public static CaseWorkerTaskAggregate ToDomain(this CaseWorkerTaskModel record)
        {
            return new CaseWorkerTaskAggregate
            {
                AggregateId = record.Id,
                Version = record.Version,
                CasePlanInstanceId = record.CasePlanInstanceId,
                CasePlanInstanceElementId = record.CasePlanInstanceElementId,
                CreateDateTime = record.CreateDateTime,
                UpdateDateTime = record.UpdateDateTime,
                Roles = record.Roles.Select(_ => ToCaseWorkerTaskRoleDomain(_)).ToList()
            };
        }

        public static CaseWorkerTaskRole ToCaseWorkerTaskRoleDomain(this RoleModel role)
        {
            return new CaseWorkerTaskRole
            {
                RoleId = role.RoleId,
                Claims = role.Claims.Select(_ => new KeyValuePair<string, string>(_.ClaimName, _.ClaimValue))
            };
        }

        #endregion

        #region To model

        public static CaseWorkerTaskModel ToModel(this CaseWorkerTaskAggregate record)
        {
            return new CaseWorkerTaskModel
            {
                Id = record.AggregateId,
                Version = record.Version,
                CasePlanInstanceId = record.CasePlanInstanceId,
                CasePlanInstanceElementId = record.CasePlanInstanceElementId,
                CreateDateTime = record.CreateDateTime,
                UpdateDateTime = record.UpdateDateTime,
                Roles = record.Roles.Select(_ => ToModel(_)).ToList()
            };
        }

        public static RoleModel ToModel(this CaseWorkerTaskRole role)
        {
            return new RoleModel
            {
                RoleId = role.RoleId,
                Claims = role.Claims.Select(_ => new RoleClaimModel
                {
                    ClaimName = _.Key,
                    ClaimValue = _.Value
                }).ToList(),
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow
            };
        }

        #endregion
    }
}
