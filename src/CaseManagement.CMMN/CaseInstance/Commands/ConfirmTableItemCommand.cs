namespace CaseManagement.CMMN.CaseInstance.Commands
{
    public class ConfirmTableItemCommand
    {
        public string CaseInstanceId { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public string User { get; set; }
    }
}