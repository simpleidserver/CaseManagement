namespace CaseManagement.BPMN.Domains
{
    public class TimerEventDefinition : BaseEventDefinition
    {
        /// <summary>
        ///  MUST conform to the ISO-8601 format for date and time representations
        /// </summary>
        public string TimeDate { get; set; }
        /// <summary>
        /// MUST conform to the ISO-8601 format for recurring time interval representations
        /// </summary>
        public string TimeCycle { get; set; }
        /// <summary>
        /// MUST conform to the ISO-8601 format for time interval representations
        /// </summary>
        public string TimeDuration { get; set; }

        public override EvtDefTypes Type => EvtDefTypes.TIMER;

        public override object Clone()
        {
            var result = new TimerEventDefinition
            {
                TimeCycle = TimeCycle,
                TimeDate = TimeDate,
                TimeDuration = TimeDuration
            };
            FeedElt(result);
            return result;
        }
    }
}
