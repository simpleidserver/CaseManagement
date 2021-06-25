namespace CaseManagement.BPMN.Domains
{
    public class MessageEventDefinition : BaseEventDefinition
    {
        /// <summary>
        /// The message MUST be supplied.
        /// </summary>
        public string MessageRef { get; set; }
        /// <summary>
        /// This attribute specifies the Operation that is used by the Message Event.
        /// It MUST be specified for executable Processes.
        /// </summary>
        public string OperationRef { get; set; }

        public override EvtDefTypes Type => EvtDefTypes.MESSAGE;

        public override bool IsSatisfied(ProcessInstanceAggregate processInstance, MessageToken token)
        {
            return processInstance.IsMessageCorrect(token);
        }

        public override object Clone()
        {
            var result = new MessageEventDefinition
            {
                MessageRef = MessageRef,
                OperationRef = OperationRef
            };
            FeedElt(result);
            return result;
        }
    }
}
