using System;

namespace CaseManagement.HumanTask.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException(string message) : base(message) { }
    }
}
