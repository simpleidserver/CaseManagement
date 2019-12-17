﻿using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IProcessQueryRepository
    {
        Task<ProcessAggregate> FindById(string id);
    }
}