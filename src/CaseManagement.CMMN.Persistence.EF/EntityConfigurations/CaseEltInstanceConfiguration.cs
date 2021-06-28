using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CaseEltInstanceConfiguration : IEntityTypeConfiguration<CaseEltInstance>
    {
        public void Configure(EntityTypeBuilder<CaseEltInstance> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Ignore(_ => _.EntryCriterions);
            builder.Ignore(_ => _.ExitCriterions);
            builder.Ignore(_ => _.Incoming);
            builder.Ignore(_ => _.LatestTransition);
            builder.HasOne(_ => _.ManualActivationRule).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(_ => _.RepetitionRule).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.TransitionHistories).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Children).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Properties).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
