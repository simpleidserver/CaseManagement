using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;

namespace CaseManagement.CMMN.Persistence.EF
{
    public class CaseManagementDbContext : DbContext
    {
        public CaseManagementDbContext(DbContextOptions<CaseManagementDbContext> dbContextOptions) : base(dbContextOptions) { }

        public virtual DbSet<CaseFileAggregate> CaseFiles { get; set; }
        public virtual DbSet<CasePlanInstanceAggregate> CasePlanInstances { get; set; }
        public virtual DbSet<CasePlanAggregate> CasePlans { get; set; }
        public virtual DbSet<CaseWorkerTaskAggregate> CaseWorkers { get; set; }
        public virtual DbSet<Subscription> SubscriptionLst { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
