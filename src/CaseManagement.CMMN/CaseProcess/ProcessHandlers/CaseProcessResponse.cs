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
            _parameters.Add(key, value);
        }

        public void AddParameter(string key, int value)
        {
            _parameters.Add(key, value.ToString());
        }

        public Dictionary<string, string> Parameters => _parameters;
    }
}
