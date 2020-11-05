using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;

namespace CaseManagement.HumanTask.Persistence.EF
{
    public class HumanTaskDBContext : DbContext
    {
        public HumanTaskDBContext(DbContextOptions<HumanTaskDBContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<HumanTaskDefinitionAggregate> HumanTaskDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
