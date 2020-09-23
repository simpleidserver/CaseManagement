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

        public override EvtDefTypes Type => EvtDefTypes.MESSAGE;

        public override bool IsSatisfied(BaseToken token)
        {
            var message = token as MessageToken;
            if (message == null || MessageRef == null || MessageRef.Name != message.Name)
            {
                return false;
            }

            return true;
        }

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
