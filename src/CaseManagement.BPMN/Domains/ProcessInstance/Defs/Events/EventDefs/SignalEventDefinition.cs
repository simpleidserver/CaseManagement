namespace CaseManagement.BPMN.Domains
{
    public class SignalEventDefinition : BaseEventDefinition
    {
        /// <summary>
        /// If the trigger is a Signal, then a Signal is provided.
        /// </summary>
        public Signal SignalRef { get; set; }

        public override EvtDefTypes Type => EvtDefTypes.SIGNAL;

        public override object Clone()
        {
            return new SignalEventDefinition
            {
                SignalRef = (Signal)SignalRef?.Clone()
            };
        }
    }
}
