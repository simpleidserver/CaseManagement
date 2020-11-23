using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Common
{
    public class MessageToken : ICloneable
    {
        public string Name { get; set; }
        public JObject MessageContent { get; set; }

        private static MessageToken Deserialize(string json)
        {
            var jObj = JsonConvert.DeserializeObject<MessageToken>(json);
            return jObj;
        }

        public static ICollection<MessageToken> DeserializeLst(string json)
        {
            var result = new List<MessageToken>();
            foreach (JObject jObj in JArray.Parse(json))
            {
                result.Add(Deserialize(jObj.ToString()));
            }

            return result;
        }

        public object Clone()
        {
            var result = new MessageToken
            {
                Name = Name,
                MessageContent = MessageContent
            };
            return result;
        }

        public static MessageToken EmptyMessage()
        {
            return new MessageToken();
        }

        public static MessageToken NewMessage(string name, JObject content)
        {
            return new MessageToken
            {
                MessageContent = content,
                Name = name
            };
        }
    }
}
