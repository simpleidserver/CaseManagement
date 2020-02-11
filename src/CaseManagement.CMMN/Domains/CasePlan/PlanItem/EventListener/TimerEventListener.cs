namespace CaseManagement.CMMN.Domains
{
    public class TimerEventListener
    {
        public TimerEventListener(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public CMMNExpression TimerExpression { get; set; }
    }
}
