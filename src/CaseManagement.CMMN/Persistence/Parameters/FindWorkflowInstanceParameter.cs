﻿using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindWorkflowInstanceParameter : BaseFindParameter
    {
        public FindWorkflowInstanceParameter() : base()
        {

        }

        public string CasePlanId { get; set; }
        public string CaseOwner { get; set; }
        public ICollection<string> Roles { get; set; }
        public bool TakeLatest { get; set; }
    }
}