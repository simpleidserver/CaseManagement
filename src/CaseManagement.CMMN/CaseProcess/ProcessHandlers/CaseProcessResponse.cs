using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public class CaseProcessResponse
    {
        private readonly Dictionary<string, string> _parameters;

        public CaseProcessResponse()
        {
            _parameters = new Dictionary<string, string>();
        }

        public void AddParameter(string key, string value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
                return;
            }

            _parameters.Add(key, value);
        }

        public void AddParameter(string key, int value)
        {
            AddParameter(key, value.ToString());
        }

        public Dictionary<string, string> Parameters => _parameters;
    }
}
