using CaseManagement.Common.Expression;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ConditionalExpressionContext : ExpressionExecutionContext
    {
        private readonly IEnumerable<MessageToken> _incomingTokens;

        public ConditionalExpressionContext(IEnumerable<MessageToken> incomingTokens)
        {
            _incomingTokens = incomingTokens;
        }

        public int GetIntIncomingMessage(string name, string path)
        {
            var result = GetIncomingMessage(name, path);
            int r;
            if (string.IsNullOrWhiteSpace(result) || !int.TryParse(result, out r))
            {
                return default(int);
            }

            return r;
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

        private string GetIncomingMessage(MessageToken incoming, string path)
        {
            var result = incoming.JObjMessageContent.SelectToken(path);
            if (result == null)
            {
                return null;
            }

            return result.ToString();
        }
    }
}
