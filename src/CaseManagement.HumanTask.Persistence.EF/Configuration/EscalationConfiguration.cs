using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class EscalationConfiguration : IEntityTypeConfiguration<Escalation>
    {
        public void Configure(EntityTypeBuilder<Escalation> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasMany(_ => _.ToParts).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
