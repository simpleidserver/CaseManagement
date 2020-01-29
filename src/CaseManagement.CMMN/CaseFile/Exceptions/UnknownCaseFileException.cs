using System;

namespace CaseManagement.CMMN.CaseFile.Exceptions
{
    public class UnknownCaseFileException : Exception
    {
        public UnknownCaseFileException(string caseFileId)
        {
            CaseFileId = caseFileId;
        }

        public string CaseFileId { get; set; }
    }
}
