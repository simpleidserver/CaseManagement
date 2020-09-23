namespace CaseManagement.BPMN.Domains
{
    public class MessageToken : BaseToken
    {
        public override TokenTypes Type => TokenTypes.Message;

        public override object Clone()
        {
            var result = new MessageToken();
            FeedToken(result);
            return result;
        }

        public static MessageToken EmptyMessage()
        {
            return new MessageToken();
        }
    }
}
