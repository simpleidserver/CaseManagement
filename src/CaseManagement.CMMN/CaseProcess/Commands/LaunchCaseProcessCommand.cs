using System.Collections.Generic;

namespace CaseManagement.CMMN.CaseProcess.Commands
{
    public class LaunchCaseProcessCommand
    {
        public LaunchCaseProcessCommand()
        {
            Parameters = new Dictionary<string, string>();
        }

        public string Id { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
