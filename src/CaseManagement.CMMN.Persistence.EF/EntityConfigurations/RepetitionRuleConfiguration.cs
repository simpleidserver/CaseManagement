using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class RepetitionRuleConfiguration : IEntityTypeConfiguration<RepetitionRule>
    {
        public void Configure(EntityTypeBuilder<RepetitionRule> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Property(_ => _.Condition).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new CMMNExpression() : JsonConvert.DeserializeObject<CMMNExpression>(v));
        }
    }
}
