using System;

namespace CaseManagement.HumanTask.Exceptions
{
    public class UnknownHumanTaskInstanceException : Exception
    {
        public UnknownHumanTaskInstanceException(string message) : base(message) { }
    }
}
