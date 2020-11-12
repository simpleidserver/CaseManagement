using Newtonsoft.Json.Linq;

namespace CaseManagement.BPMN.Common
{
    public class StateTransitionToken : BaseToken
    {
        public override TokenTypes Type => TokenTypes.StateTransition;

        public string State { get; set; }
        public string FlowNodeInstanceId { get; set; }
        public JObject Content { get; set; }

        public override object Clone()
        {
            var result = new StateTransitionToken();
            FeedToken(result);
            result.State = State;
            result.Content = Content;
            FlowNodeInstanceId = FlowNodeInstanceId;
            return result;
        }
    }
}
