using System;

namespace CaseManagement.BPMN.Exceptions
{
    public class UnknownProcessFileException : Exception
    {
        public UnknownProcessFileException(string msg) : base(msg)
        {

        }
    }
}
