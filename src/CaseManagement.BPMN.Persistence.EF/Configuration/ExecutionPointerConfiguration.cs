using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ExecutionPointerConfiguration : IEntityTypeConfiguration<ExecutionPointer>
    {
        public void Configure(EntityTypeBuilder<ExecutionPointer> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Ignore(_ => _.Incoming);
            builder.Ignore(_ => _.Outgoing);
            builder.HasMany(_ => _.Tokens).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
