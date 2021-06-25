using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class FlowNodeInstanceConfiguration : IEntityTypeConfiguration<FlowNodeInstance>
    {
        public void Configure(EntityTypeBuilder<FlowNodeInstance> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Property(_ => _.Metadata).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            builder.HasMany(_ => _.ActivityStates).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
