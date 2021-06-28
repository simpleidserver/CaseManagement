using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
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

        public Task<Subscription> Get(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            return _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId && _.EventName == evtName, token);
        }

        public async Task<Subscription> TryReset(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId && _.EventName == evtName, token);
            if (result == null)
            {
                return null;
            }

            result.IsCaptured = false;
            result.CaptureDateTime = null;
            await _dbContext.SaveChangesAsync(token);
            return result;
        }

        public async Task<Subscription> TrySubscribe(string casePlanInstanceId, string evtName, CancellationToken token)
        {
            var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == null && _.EventName == evtName, token);
            if (result == null)
            {
                result = new Subscription
                {
                    CasePlanInstanceId = casePlanInstanceId,
                    EventName = evtName,
                    CreationDateTime = DateTime.UtcNow,
                    IsCaptured = false
                };
                _dbContext.SubscriptionLst.Add(result);
                await _dbContext.SaveChangesAsync(token);
            }

            return result;
        }

        public async Task<Subscription> TrySubscribe(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            var result = await _dbContext.SubscriptionLst.FirstOrDefaultAsync(_ => _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId && _.EventName == evtName, token);
            if (result == null)
            {
                result = new Subscription
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

            return result;
        }

        public async Task<bool> Update(Subscription subscription, CancellationToken cancellationToken)
        {
            _dbContext.SubscriptionLst.Update(subscription);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}