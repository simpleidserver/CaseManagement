using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Domains.DelegateConfiguration;
using Microsoft.EntityFrameworkCore;

namespace CaseManagement.BPMN.Persistence.EF
{
    public class BPMNDbContext : DbContext
    {
        public BPMNDbContext(DbContextOptions<BPMNDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<ProcessInstanceAggregate> ProcessInstances { get; set; }
        public DbSet<ProcessFileAggregate> ProcessFiles { get; set; }   
        public DbSet<DelegateConfigurationAggregate> DelegateConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
