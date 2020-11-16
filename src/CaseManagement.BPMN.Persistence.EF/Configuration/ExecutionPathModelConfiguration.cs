using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ExecutionPathModelConfiguration : IEntityTypeConfiguration<ExecutionPathModel>
    {
        public void Configure(EntityTypeBuilder<ExecutionPathModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.HasMany(_ => _.Pointers).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
