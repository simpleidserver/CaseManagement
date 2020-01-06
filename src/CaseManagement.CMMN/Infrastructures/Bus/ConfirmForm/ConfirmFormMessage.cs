namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm
{
    public class ConfirmFormMessage
    {
        public ConfirmFormMessage(string caseInstanceId, string caseElementInstanceId, string formInstanceId)
        {
            CaseInstanceId = caseInstanceId;
            CaseElementInstanceId = caseElementInstanceId;
            FormInstanceId = formInstanceId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
        public string FormInstanceId { get; set; }
    }
}
