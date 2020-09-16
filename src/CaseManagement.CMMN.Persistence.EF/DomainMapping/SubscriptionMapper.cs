using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Persistence.EF.Models;

namespace CaseManagement.CMMN.Persistence.EF.DomainMapping
{
    public static class SubscriptionMapper
    {
        public static Subscription ToDomain(this SubscriptionModel sub)
        {
            return new Subscription
            {
                CaptureDateTime = sub.CaptureDateTime,
                CreationDateTime = sub.CreationDateTime,
                EventName = sub.EventName,
                CasePlanElementInstanceId = sub.CasePlanElementInstanceId,
                CasePlanInstanceId = sub.CasePlanInstanceId,
                IsCaptured = sub.IsCaptured
            };
        }

        public static SubscriptionModel ToModel(this Subscription sub)
        {
            return new SubscriptionModel
            {
                CaptureDateTime = sub.CaptureDateTime,
                CreationDateTime = sub.CreationDateTime,
                EventName = sub.EventName,
                CasePlanElementInstanceId = sub.CasePlanElementInstanceId,
                CasePlanInstanceId = sub.CasePlanInstanceId,
                IsCaptured = sub.IsCaptured
            };
        }
    }
}
