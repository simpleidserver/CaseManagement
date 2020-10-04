using System;

namespace CaseManagement.Common.ISO8601
{
    public class ISO8601TimeInterval
    {
        public ISO8601TimeInterval(DateTime startDateTime, DateTime endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
