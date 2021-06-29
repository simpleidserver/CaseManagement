using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class SEntryConfiguration : IEntityTypeConfiguration<SEntry>
    {
        public void Configure(EntityTypeBuilder<SEntry> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.HasMany(_ => _.PlanItemOnParts).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.FileItemOnParts).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(_ => _.IfPart).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new IfPart() : JsonConvert.DeserializeObject<IfPart>(v));
        }
    }
}
