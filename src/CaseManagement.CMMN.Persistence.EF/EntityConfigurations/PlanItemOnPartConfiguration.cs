using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class PlanItemOnPartConfiguration : IEntityTypeConfiguration<PlanItemOnPart>
    {
        public void Configure(EntityTypeBuilder<PlanItemOnPart> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Ignore(_ => _.Type);
            builder.Property(_ => _.IncomingTokens).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>(): JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
        }
    }
}
