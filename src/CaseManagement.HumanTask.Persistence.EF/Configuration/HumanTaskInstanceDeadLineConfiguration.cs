using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskInstanceDeadLineConfiguration : IEntityTypeConfiguration<HumanTaskInstanceDeadLine>
    {
        public void Configure(EntityTypeBuilder<HumanTaskInstanceDeadLine> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.HasMany(_ => _.Escalations).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
