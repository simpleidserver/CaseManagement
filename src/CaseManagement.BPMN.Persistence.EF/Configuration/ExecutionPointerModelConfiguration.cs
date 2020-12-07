using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ExecutionPointerModelConfiguration : IEntityTypeConfiguration<ExecutionPointerModel>
    {
        public void Configure(EntityTypeBuilder<ExecutionPointerModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedNever();
            builder.Ignore(_ => _.Incoming);
            builder.Ignore(_ => _.Outgoing);
            builder.HasMany(_ => _.Tokens).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
