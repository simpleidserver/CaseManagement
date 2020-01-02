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
        public abstract string ReadContent();
        public abstract IEnumerable<CaseFileItem> GetChildren();
    }
}
