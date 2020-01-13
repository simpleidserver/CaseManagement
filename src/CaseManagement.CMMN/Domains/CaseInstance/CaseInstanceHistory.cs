using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseInstanceHistory : ICloneable
    {
        public CaseInstanceHistory(string state, DateTime updateDateTime)
        {
            State = state;
            UpdateDateTime = updateDateTime;
        }

        public string State { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public object Clone()
        {
            return new CaseInstanceHistory(State, UpdateDateTime);
        }
    }
}
