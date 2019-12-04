using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseProcess.Commands
{
    public class LaunchCaseProcessCommand
    {
        public LaunchCaseProcessCommand(string id, Dictionary<string, string> parameters)
        {
            Id = id;
            Parameters = parameters;
        }

        public string Id { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
