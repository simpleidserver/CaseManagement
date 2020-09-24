using Newtonsoft.Json.Linq;

namespace CaseManagement.BPMN.Common
{
    public class MessageToken : BaseToken
    {
        public override TokenTypes Type => TokenTypes.Message;
        public JObject MessageContent { get; set; }

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
