using CaseManagement.Common.Parameters;

namespace CaseManagement.HumanTask.Persistence.Parameters
{
    public class FindNotificationInstanceParameter : BaseSearchParameter
    {
        public UserClaims User { get; set; }
    }
}
