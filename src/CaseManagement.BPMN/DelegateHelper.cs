using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CaseManagement.BPMN
{
    public static class DelegateHelper
    {
        public static string Parse(DelegateConfigurationAggregate configuration, ICollection<MessageToken> incoming, string message)
        {
            var regularExpression = new Regex(@"{{([a-zA-Z]|_|\.|\(|\)|\'|\ |\,)*\}}");
            var isMessageRegularExpression = new Regex(@"messages.Get\((\'([a-zA-Z]|\.|\ |\,)*\')");
            var isConfigurationRegularExpression = new Regex(@"configuration.Get\((\'([a-zA-Z])*\')");
            return regularExpression.Replace(message, (m) =>
            {
                if (string.IsNullOrWhiteSpace(m.Value))
                {
                    return string.Empty;
                }

                var str = m.Value.Replace("{{", "");
                str = str.Replace("}}", "");
                if (isMessageRegularExpression.IsMatch(str))
                {
                    str = str.Replace("messages.Get('", "");
                    str = str.TrimEnd(')');
                    str = str.Replace("'", "");
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return null;
                    }

                    var parameters = str.Split(',');
                    if (parameters.Count() != 2)
                    {
                        return null;
                    }

                    var firstParameter = parameters.First().Replace(" ", "");
                    var secondParameter = parameters.Last().Replace(" ", "");
                    var token = incoming.FirstOrDefault(i => i.Name == firstParameter);
                    if (token == null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(secondParameter))
                    {
                        return null;
                    }

                    var jsonToken = token.JObjMessageContent.SelectToken(secondParameter);
                    if (jsonToken == null)
                    {
                        return null;
                    }

                    return jsonToken.ToString();
                }

                if (isConfigurationRegularExpression.IsMatch(str))
                {
                    str = str.Replace("configuration.Get('", "");
                    str = str.TrimEnd(')');
                    str = str.Replace("'", "");
                    return configuration.GetValue(str);
                }

                return null;
            });
        }
    }
}
