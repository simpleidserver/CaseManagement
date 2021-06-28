using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Property(_ => _.Parameters).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
        }
    }
}
