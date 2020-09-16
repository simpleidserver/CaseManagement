using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.Models;
using System;
using System.Linq;

namespace CaseManagement.CMMN.Persistence.EF.DomainMapping
{
    public static class CasePlanMapper
    {
        #region To domain

        public static CasePlanAggregate ToDomain(this CasePlanModel casePlan)
        {
            return new CasePlanAggregate
            {
                AggregateId = casePlan.Id,
                Version = casePlan.Version,
                CasePlanId = casePlan.CasePlanId,
                Name = casePlan.Name,
                Description = casePlan.Description,
                CaseOwner = casePlan.CaseOwner,
                CaseFileId = casePlan.CaseFileId,
                CreateDateTime = casePlan.CreateDateTime,
                Roles = casePlan.Roles.Select(_ => ToCasePlanRoleDomain(_)).ToList(),
                Files = casePlan.Files.Select(_ => ToDomain(_)).ToList(),
                XmlContent = casePlan.SerializedContent
            };
        }

        public static CasePlanRole ToCasePlanRoleDomain(this RoleModel role)
        {
            return new CasePlanRole
            {
                Id = role.RoleId,
                Name = role.RoleName
            };
        }

        public static CasePlanFileItem ToDomain(this CasePlanFileItemModel fileItem)
        {
            return new CasePlanFileItem
            {
                Id = fileItem.FileItemId,
                Name = fileItem.FileItemName,
                DefinitionType = fileItem.DefinitionType,
            };
        }

        #endregion

        #region To model

        public static CasePlanModel ToModel(this CasePlanAggregate casePlan)
        {
            return new CasePlanModel
            {
                Id = casePlan.AggregateId,
                Version = casePlan.Version,
                CasePlanId = casePlan.CasePlanId,
                Name = casePlan.Name,
                Description = casePlan.Description,
                CaseOwner = casePlan.CaseOwner,
                CaseFileId = casePlan.CaseFileId,
                CreateDateTime = casePlan.CreateDateTime,
                Roles = casePlan.Roles.Select(_ => ToModel(_, casePlan.AggregateId)).ToList(),
                Files = casePlan.Files.Select(_ => ToModel(_, casePlan.AggregateId)).ToList(),
                SerializedContent = casePlan.XmlContent
            };
        }

        public static RoleModel ToModel(this CasePlanRole role, string casePlanId)
        {
            return new RoleModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                CasePlanId = casePlanId,
                CreateDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow
            };
        }

        public static CasePlanFileItemModel ToModel(this CasePlanFileItem fileItem, string casePlanId)
        {
            return new CasePlanFileItemModel
            {
                FileItemId = fileItem.Id,
                FileItemName = fileItem.Name,
                CasePlanId = casePlanId,
                DefinitionType = fileItem.DefinitionType
            };
        }

        #endregion
    }
}
