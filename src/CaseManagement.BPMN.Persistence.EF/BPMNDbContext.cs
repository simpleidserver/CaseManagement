using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseManagement.BPMN.Persistence.EF
{
    public class BPMNDbContext : DbContext
    {
        public BPMNDbContext(DbContextOptions<BPMNDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<ProcessInstanceModel> ProcessInstances { get; set; }
        public DbSet<ProcessFileModel> ProcessFiles { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
