﻿using System.Collections.Generic;
using CaseManagement.CMMN.CaseInstance.Repositories;
using PortCMIS.Client;

namespace CaseManagement.CMMN.CMIS
{
    public class CMISDirectoryCaseFileItem : CaseFileItem
    {
        private readonly ICMISSessionFactory _cmisSessionFactory;

        public CMISDirectoryCaseFileItem(ICMISSessionFactory cmisSessionFactory, string id) : base(id)
        {
            _cmisSessionFactory = cmisSessionFactory;
        }

        public override IEnumerable<CaseFileItem> GetChildren()
        {
            var session = _cmisSessionFactory.GetSession();
            var obj = session.GetObject(Id) as IFolder;
            foreach(var child in obj.GetChildren())
            {
                if (child is IFolder)
                {
                    yield return new CMISDirectoryCaseFileItem(_cmisSessionFactory, child.Id);
                }
                else if (child is IDocument)
                {
                    yield return new CMISDocumentCaseFileItem(_cmisSessionFactory, child.Id);
                }
            }
        }

        public override string ReadContent()
        {
            return string.Empty;
        }
    }
}
