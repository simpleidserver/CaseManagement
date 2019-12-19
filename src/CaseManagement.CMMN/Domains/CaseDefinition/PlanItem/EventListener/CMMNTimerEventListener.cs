using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNTimerEventListener : ICloneable
    {
        public CMMNTimerEventListener(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public CMMNExpression TimerExpression { get; set; }

        public object Clone()
        {
            return new CMMNTimerEventListener(Name)
            {
                TimerExpression = TimerExpression == null ? null : (CMMNExpression)TimerExpression.Clone()
            };
        }
    }
}
