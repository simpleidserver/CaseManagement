using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF
{
    public class CaseManagementDbContext : DbContext
    {
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public CaseManagementDbContext(DbContextOptions<CaseManagementDbContext> dbContextOptions) : base(dbContextOptions) { }

        internal async Task<DbContextLock> Lock()
        {
            await _semaphoreSlim.WaitAsync();
            return new DbContextLock(_semaphoreSlim);
        }

        public virtual DbSet<Models.RoleClaimModel> Claims { get; set; }
        public virtual DbSet<Models.RoleModel> Roles { get; set; }
        public virtual DbSet<Models.CaseFileModel> CaseFiles { get; set; }
        public virtual DbSet<Models.CasePlanInstanceModel> CasePlanInstances { get; set; }
        public virtual DbSet<Models.CasePlanModel> CasePlans { get; set; }
        public virtual DbSet<Models.CaseWorkerTaskModel> CaseWorkers { get; set; }
        public virtual DbSet<Models.QueueMessageModel> QueueMessageLst { get; set; }
        public virtual DbSet<Models.ScheduledMessageModel> ScheduledMessageLst { get; set; }
        public virtual DbSet<Models.SubscriptionModel> SubscriptionLst { get; set; }
        public virtual DbSet<Models.CasePlanInstanceFileItemModel> CasePlanInstanceFileItemLst { get; set; }
        public virtual DbSet<Models.CasePlanInstanceWorkerTaskModel> CasePlanInstanceWorkerTaskLst { get; set; }
        public virtual DbSet<Models.CasePlanElementInstanceModel> CasePlanElementInstanceLst { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }

    internal class DbContextLock : IDisposable
    {
        private readonly SemaphoreSlim _semaphoreSlim;

        public DbContextLock(SemaphoreSlim semaphoreSlim)
        {
            _semaphoreSlim = semaphoreSlim;
        }

        public void Dispose()
        {
            _semaphoreSlim.Release();
        }
    }
}
