using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Persistence.EF.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

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
                IsCaptured = sub.IsCaptured,
                Parameters = string.IsNullOrEmpty(sub.Parameters) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>> (sub.Parameters)
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
                IsCaptured = sub.IsCaptured,
                Parameters = sub.Parameters == null ? null : JsonConvert.SerializeObject(sub.Parameters)
            };
        }
    }
}
