using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Common
{
    public abstract class BaseToken
    {
        public string Name { get; set; }
        public abstract TokenTypes Type { get; }

        public abstract object Clone();

        protected void FeedToken(BaseToken token)
        {
            token.Name = Name;
        }

        private static BaseToken Deserialize(string json)
        {
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            var type = (TokenTypes)int.Parse(jObj["Type"].ToString());
            switch (type)
            {
                case TokenTypes.Message:
                    return JsonConvert.DeserializeObject<MessageToken>(json);
            }

            return null;
        }

        public static ICollection<BaseToken> DeserializeLst(string json)
        {
            var result = new List<BaseToken>();
            foreach (JObject jObj in JArray.Parse(json))
            {
                result.Add(Deserialize(jObj.ToString()));
            }

            return result;
        }
    }
}
