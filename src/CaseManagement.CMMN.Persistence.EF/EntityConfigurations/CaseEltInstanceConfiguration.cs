using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

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
            builder.HasMany(_ => _.TransitionHistories).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Children).WithOne();
            builder.HasMany(_ => _.Properties).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Criterias).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(_ => _.ManualActivationRule).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new ManualActivationRule() : JsonConvert.DeserializeObject<ManualActivationRule>(v));
            builder.Property(_ => _.RepetitionRule).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new RepetitionRule() : JsonConvert.DeserializeObject<RepetitionRule>(v));
        }
    }
}
