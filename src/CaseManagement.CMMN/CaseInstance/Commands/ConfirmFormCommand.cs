using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseInstance.Commands
{
    [DataContract]
    public class ConfirmFormCommand
    {
        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
    }
}
