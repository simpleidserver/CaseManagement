using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class FlowNodeInstanceModelConfiguration : IEntityTypeConfiguration<FlowNodeInstanceModel>
    {
        public void Configure(EntityTypeBuilder<FlowNodeInstanceModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Metadata).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            builder.HasMany(_ => _.ActivityStates).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
