using System;

namespace CaseManagement.CMMN.Domains
{
    public class TimerEventListener : ICloneable
    {
        public TimerEventListener(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public CMMNExpression TimerExpression { get; set; }

        public object Clone()
        {
            return new TimerEventListener(Name)
            {
                TimerExpression = TimerExpression == null ? null : (CMMNExpression)TimerExpression.Clone()
            };
        }
    }
}
