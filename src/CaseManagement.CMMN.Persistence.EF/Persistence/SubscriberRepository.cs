using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly CaseManagementDbContext _dbContext;

        public SubscriberRepository(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Subscription> Get(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            using (await _dbContext.Lock())
            {
                var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId && _.EventName == evtName, token);
                return result == null ? null : result.ToDomain();
            }
        }

        public async Task<Subscription> TryReset(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            using (await _dbContext.Lock())
            {
                var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId && _.EventName == evtName, token);
                if (result == null)
                {
                    return null;
                }

                result.IsCaptured = false;
                result.CaptureDateTime = null;
                await _dbContext.SaveChangesAsync(token);
                return result.ToDomain();
            }
        }

        public async Task<Subscription> TrySubscribe(string casePlanInstanceId, string evtName, CancellationToken token)
        {
            using (await _dbContext.Lock())
            {
                var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == null && _.EventName == evtName, token);
                if (result == null)
                {
                    result = new SubscriptionModel
                    {
                        CasePlanInstanceId = casePlanInstanceId,
                        EventName = evtName,
                        CreationDateTime = DateTime.UtcNow,
                        IsCaptured = false
                    };
                    _dbContext.SubscriptionLst.Add(result);
                    await _dbContext.SaveChangesAsync(token);
                }

                return result.ToDomain();
            }
        }

        public async Task<Subscription> TrySubscribe(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            using (await _dbContext.Lock())
            {
                var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId && _.EventName == evtName, token);
                if (result == null)
                {
                    result = new SubscriptionModel
                    {
                        CasePlanInstanceId = casePlanInstanceId,
                        EventName = evtName,
                        CreationDateTime = DateTime.UtcNow,
                        CasePlanElementInstanceId = casePlanElementInstanceId,
                        IsCaptured = false
                    };
                    _dbContext.SubscriptionLst.Add(result);
                    await _dbContext.SaveChangesAsync(token);
                }

                return result.ToDomain();
            }
        }

        public async Task<bool> Update(Subscription subscription, CancellationToken cancellationToken)
        {
            using (await _dbContext.Lock())
            {
                var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == subscription.CasePlanInstanceId && _.CasePlanElementInstanceId == subscription.CasePlanElementInstanceId && _.EventName == subscription.EventName, cancellationToken);
                if (result == null)
                {
                    return false;
                }

                result.IsCaptured = subscription.IsCaptured;
                result.CaptureDateTime = subscription.CaptureDateTime;
                result.Parameters = subscription.Parameters == null ? null : JsonConvert.SerializeObject(subscription.Parameters);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
