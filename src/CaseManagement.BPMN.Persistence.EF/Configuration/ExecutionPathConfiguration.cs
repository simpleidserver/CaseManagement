using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ExecutionPathConfiguration : IEntityTypeConfiguration<ExecutionPath>
    {
        public void Configure(EntityTypeBuilder<ExecutionPath> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Ignore(_ => _.ActivePointers);
            builder.HasMany(_ => _.Pointers).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
