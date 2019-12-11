using CaseManagement.CMMN.CaseInstance.Repositories;
using PortCMIS.Client;
using System.Collections.Generic;
using System.IO;

namespace CaseManagement.CMMN.CMIS
{
    public class CMISDocumentCaseFileItem : CaseFileItem
    {
        private readonly ICMISSessionFactory _cmisSessionFactory;

        public CMISDocumentCaseFileItem(ICMISSessionFactory cmisSessionFactory, string id) : base(id)
        {
            _cmisSessionFactory = cmisSessionFactory;
        }

        public override IEnumerable<CaseFileItem> GetChildren()
        {
            return new CaseFileItem[0];
        }

        public override string ReadContent()
        {
            var session = _cmisSessionFactory.GetSession();
            var obj = session.GetObject(Id) as IDocument;
            using (var reader = new StreamReader(obj.GetContentStream().Stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
