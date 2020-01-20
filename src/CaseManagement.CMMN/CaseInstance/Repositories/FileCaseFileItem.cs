using System.Collections.Generic;
using System.IO;

namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    public class FileCaseFileItem : CaseFileItem
    {
        public FileCaseFileItem(string id) : base(id, CMMNConstants.ContentManagementTypes.FILE)
        {
        }

        public override string ReadContent()
        {
            return File.ReadAllText(Id);
        }

        public override IEnumerable<CaseFileItem> GetChildren()
        {
            return new CaseFileItem[0];
        }
    }
}
