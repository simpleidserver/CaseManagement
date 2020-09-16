
namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CasePlanFileItemModel
    {
        public long Id { get; set; }
        public string CasePlanId { get; set; }
        public string FileItemId { get; set; }
        public string FileItemName { get; set; }
        public string DefinitionType { get; set; }
        public virtual CasePlanModel CasePlan { get; set; }
    }
}
