using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm
{
    public class ConfirmFormMessage
    {
        public ConfirmFormMessage(string caseInstanceId, string caseElementInstanceId, string formInstanceId, Dictionary<string, string> formValues)
        {
            CaseInstanceId = caseInstanceId;
            CaseElementInstanceId = caseElementInstanceId;
            FormInstanceId = formInstanceId;
            FormValues = formValues;
        }

        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
        public string FormInstanceId { get; set; }
        public Dictionary<string, string> FormValues { get; set; }
    }
}
