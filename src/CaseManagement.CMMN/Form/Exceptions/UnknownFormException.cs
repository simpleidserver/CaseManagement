using System;

namespace CaseManagement.CMMN.Form.Exceptions
{
    public class UnknownFormException : Exception
    {
        public UnknownFormException(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
