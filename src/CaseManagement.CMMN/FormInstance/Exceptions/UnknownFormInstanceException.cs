using System;

namespace CaseManagement.CMMN.FormInstance.Exceptions
{
    public class UnknownFormInstanceException : Exception
    {
        public UnknownFormInstanceException(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
