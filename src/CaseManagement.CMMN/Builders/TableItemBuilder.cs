using CaseManagement.CMMN.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseManagement.CMMN.Builders
{
    public class TableItemBuilder
    {
        private readonly TableItem _tableItem;

        public TableItemBuilder(TableItem tableItem)
        {
            _tableItem = tableItem;
        }
        
        public TableItemBuilder SetAuthorizedRole(string name)
        {
            _tableItem.AuthorizedRoleRef = name;
            return this;
        }

        public TableItemBuilder SetApplicabilityRule(string name, string contextRef, string expression)
        {
            _tableItem.ApplicabilityRuleRef = new ApplicabilityRule { Name = name, ContextRef = contextRef, Expression = expression };
            return this;
        }
    }
}
