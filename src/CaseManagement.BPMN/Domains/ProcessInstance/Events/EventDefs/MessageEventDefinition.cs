namespace CaseManagement.BPMN.Domains
{
    public class MessageEventDefinition : BaseEventDefinition
    {
        /// <summary>
        /// The message MUST be supplied.
        /// </summary>
        public Message MessageRef { get; set; }
        /// <summary>
        /// This attribute specifies the Operation that is used by the Message Event.
        /// It MUST be specified for executable Processes.
        /// </summary>
        public Operation OperationRef { get; set; }

        public override object Clone()
        {
            var result = new MessageEventDefinition
            {
                MessageRef = (Message)MessageRef.Clone(),
                OperationRef = (Operation)OperationRef.Clone()
            };
            FeedElt(result);
            return result;
        }
    }
}
