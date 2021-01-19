using System;

namespace CaseManagement.HumanTask.Exceptions
{
    public class UnknownNotificationDefException : Exception
    {
        public UnknownNotificationDefException(string msg) : base(msg)
        {

        }
    }
}
