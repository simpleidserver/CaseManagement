using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementInstanceHistory : ICloneable
    {
        public CaseElementInstanceHistory(string state, DateTime updateDateTime)
        {
            State = state;
            UpdateDateTime = updateDateTime;
        }

        public string State { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public object Clone()
        {
            return new CaseElementInstanceHistory(State, UpdateDateTime);
        }
    }
}
