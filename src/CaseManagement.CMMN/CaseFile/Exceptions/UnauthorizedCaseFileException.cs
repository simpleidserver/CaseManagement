using System;

namespace CaseManagement.CMMN.CaseFile.Exceptions
{
    public class UnauthorizedCaseFileException : Exception
    {
        public UnauthorizedCaseFileException(string nameIdentifier, string caseFileId)
        {
            NameIdentifier = nameIdentifier;
            CaseFileId = caseFileId;
        }

        public string NameIdentifier { get; set; }
        public string CaseFileId { get; set; }
    }
}
