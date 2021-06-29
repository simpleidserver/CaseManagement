using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanInstanceAggregateConfiguration : IEntityTypeConfiguration<CasePlanInstanceAggregate>
    {
        public void Configure(EntityTypeBuilder<CasePlanInstanceAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
            builder.Ignore(_ => _.ExecutionContext);
            builder.Ignore(_ => _.StageContent);
            builder.Ignore(_ => _.FileItems);
            builder.Property(_ => _.ExecutionContextVariables).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            builder.HasMany(_ => _.Roles).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Files).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.WorkerTasks).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Children).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}