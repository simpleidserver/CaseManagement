using System;

namespace CaseManagement.HumanTask.Exceptions
{
    public class UnknownHumanTaskDefException : Exception
    {
        public UnknownHumanTaskDefException(string message) : base(message) { }
    }
}
