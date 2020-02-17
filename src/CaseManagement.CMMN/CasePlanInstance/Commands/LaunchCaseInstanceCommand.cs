using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class LaunchCaseInstanceCommand
    {
        public bool BypassUserValidation { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string Performer { get; set; }
    }
}
