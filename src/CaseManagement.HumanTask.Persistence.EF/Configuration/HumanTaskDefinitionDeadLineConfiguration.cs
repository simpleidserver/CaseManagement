using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskDefinitionDeadLineConfiguration : IEntityTypeConfiguration<HumanTaskDefinitionDeadLine>
    {
        public void Configure(EntityTypeBuilder<HumanTaskDefinitionDeadLine> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasMany(_ => _.Escalations).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
