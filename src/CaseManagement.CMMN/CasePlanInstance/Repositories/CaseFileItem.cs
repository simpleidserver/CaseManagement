using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Repositories
{
    public abstract class CaseFileItem
    {
        public CaseFileItem(string id, string type)
        {
            Id = id;
            Type = type;
        }

        public string Id { get; set; }
        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public string Type { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Value { get; set; }
        public abstract string ReadContent();
        public abstract IEnumerable<CaseFileItem> GetChildren();
    }
}
