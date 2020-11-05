using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskDefinitionSubTaskConfiguration : IEntityTypeConfiguration<HumanTaskDefinitionSubTask>
    {
        public void Configure(EntityTypeBuilder<HumanTaskDefinitionSubTask> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            // builder.Ignore(_ => _.ToParts);
            builder.HasMany(_ => _.ToParts).WithOne();
        }
    }
}