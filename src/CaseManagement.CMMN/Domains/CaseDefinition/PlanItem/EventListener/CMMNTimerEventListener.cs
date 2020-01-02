namespace CaseManagement.CMMN.Domains
{
    public class CMMNTimerEventListener
    {
        public CMMNTimerEventListener(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public CMMNExpression TimerExpression { get; set; }
    }
}
