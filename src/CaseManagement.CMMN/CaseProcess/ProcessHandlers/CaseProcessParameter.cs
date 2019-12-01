using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public class CaseProcessParameter
    {
        private Dictionary<string, string> _parameters;

        public CaseProcessParameter(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
        }

        public string GetStringParameter(string key)
        {
            if (!_parameters.ContainsKey(key))
            {
                return null;
            }

            return _parameters[key];
        }

        public int GetIntParameter(string key)
        {
            var value = GetStringParameter(key);
            if (string.IsNullOrWhiteSpace(key))
            {
                return default(int);
            }

            int i;
            if (int.TryParse(value, out i))
            {
                return i;
            }

            return default(int);
        }
    }
}
