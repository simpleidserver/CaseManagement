
namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class RoleClaimModel
    {
        public long Id { get; set; }
        public long? RoleId { get; set; }
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }
        public virtual RoleModel Role { get; set; }
    }
}