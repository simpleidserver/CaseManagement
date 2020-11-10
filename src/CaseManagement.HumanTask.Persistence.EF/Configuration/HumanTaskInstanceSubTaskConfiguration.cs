using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskInstanceSubTaskConfiguration : IEntityTypeConfiguration<HumanTaskInstanceSubTask>
    {
        public void Configure(EntityTypeBuilder<HumanTaskInstanceSubTask> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.HasMany(_ => _.ToParts).WithOne();
        }
    }
}
