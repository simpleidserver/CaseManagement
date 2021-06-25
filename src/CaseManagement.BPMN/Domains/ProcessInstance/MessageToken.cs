using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class MessageToken : ICloneable
    {
        public string Name { get; set; }
        public string MessageContent { get; set; }
        public MessageTokenTypes Type { get; set; }

        [field: NonSerialized]
        public JObject JObjMessageContent
        {
            get
            {
                return string.IsNullOrWhiteSpace(MessageContent) ? new JObject() : JObject.Parse(MessageContent);
            }
        }

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
                MessageContent = MessageContent,
                Type = Type
            };
            return result;
        }

        public static MessageToken EmptyMessage()
        {
            return new MessageToken();
        }

        public static MessageToken NewMessage(string name, string content)
        {
            return new MessageToken
            {
                MessageContent = content,
                Name = name
            };
        }
    }
}
