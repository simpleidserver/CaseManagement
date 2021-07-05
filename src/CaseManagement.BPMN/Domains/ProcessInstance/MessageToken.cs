using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class MessageToken : ICloneable
    {
        public string Id { get; set; }
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

        public string GetProperty(string key)
        {
            if (!JObjMessageContent.ContainsKey(key))
            {
                return null;
            }

            return JObjMessageContent[key].ToString();
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
                Id = Id,
                Name = Name,
                MessageContent = MessageContent,
                Type = Type
            };
            return result;
        }

        public static MessageToken EmptyMessage(string id, string name)
        {
            return new MessageToken
            {
                Id = id,
                Name = name
            };
        }

        public static MessageToken NewMessage(string id, string name, string content)
        {
            return new MessageToken
            {
                Id = id,
                MessageContent = content,
                Name = name
            };
        }
    }
}
