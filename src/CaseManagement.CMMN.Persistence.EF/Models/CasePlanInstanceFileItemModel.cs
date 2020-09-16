
namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CasePlanInstanceFileItemModel
    {
        public long Id { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public string CaseFileItemType { get; set; }
        public string ExternalValue { get; set; }
        public virtual CasePlanInstanceModel CasePlanInstance { get; set; }
    }
}
