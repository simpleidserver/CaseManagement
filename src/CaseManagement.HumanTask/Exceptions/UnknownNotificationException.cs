using System;

namespace CaseManagement.HumanTask.Exceptions
{
    public class UnknownNotificationException : Exception
    {
        public UnknownNotificationException(string message) : base(message) { }
    }
}
