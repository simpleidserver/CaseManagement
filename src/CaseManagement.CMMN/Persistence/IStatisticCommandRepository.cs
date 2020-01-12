﻿using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IStatisticCommandRepository
    {
        void Update(CMMNWorkflowDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate);
        void Add(CMMNWorkflowDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate);
        Task<int> SaveChanges();
    }
}
