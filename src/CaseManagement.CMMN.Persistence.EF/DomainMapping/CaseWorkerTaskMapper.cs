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
                UpdateDateTime = record.UpdateDateTime
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
                UpdateDateTime = record.UpdateDateTime
            };
        }

        #endregion
    }
}
