using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.BPMN.Domains
{
    public class StateTransition : ICloneable
    {
        public string State { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public JObject Content { get; set; }

        public object Clone()
        {
            return new StateTransition
            {
                State = State,
                ReceivedDateTime = ReceivedDateTime,
                Content = Content
            };
        }
    }
}
