using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public abstract class CaseFileItem
    {
        public CaseFileItem(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public string Value { get; set; }
        public abstract string ReadContent();
        public abstract IEnumerable<CaseFileItem> GetChildren();
    }
}
