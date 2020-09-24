using CaseManagement.BPMN.Common;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ConditionalExpressionContext
    {
        private readonly ICollection<BaseToken> _incomingTokens;

        public ConditionalExpressionContext(ICollection<BaseToken> incomingTokens)
        {
            _incomingTokens = incomingTokens;
        }

        public string GetIncomingMessage(int incomingIndex, string path)
        {
            if (incomingIndex >= _incomingTokens.Count())
            {
                return null;
            }

            var incoming = _incomingTokens.ElementAt(incomingIndex);
            return GetIncomingMessage(incoming, path);
        }

        public string GetIncomingMessage(string name, string path)
        {
            var incoming = _incomingTokens.FirstOrDefault(_ => _.Name == name);
            if (incoming == null)
            {
                return null;
            }

            return GetIncomingMessage(incoming, path);
        }

        public string GetIncomingMessage(string path)
        {
            if (!_incomingTokens.Any())
            {
                return null;
            }

            return GetIncomingMessage(_incomingTokens.First(), path);
        }

        private string GetIncomingMessage(BaseToken incoming, string path)
        {
            var message = incoming as MessageToken;
            if (message == null)
            {
                return null;
            }

            var result = message.MessageContent.SelectToken(path);
            if (result == null)
            {
                return null;
            }

            return result.ToString();
        }
    }
}
