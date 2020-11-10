using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskInstanceConfiguration : IEntityTypeConfiguration<HumanTaskInstanceAggregate>
    {
        public void Configure(EntityTypeBuilder<HumanTaskInstanceAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Property(_ => _.InputParameters).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            builder.Property(_ => _.OutputParameters).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            builder.Ignore(_ => _.InputOperationParameters);
            builder.Ignore(_ => _.OutputOperationParameters);
            builder.Ignore(_ => _.DomainEvents);
            builder.Ignore(_ => _.Names);
            builder.Ignore(_ => _.Subjects);
            builder.Ignore(_ => _.Descriptions);
            builder.Ignore(_ => _.PotentialOwners);
            builder.Ignore(_ => _.BusinessAdministrators);
            builder.Ignore(_ => _.ExcludedOwners);
            builder.Ignore(_ => _.TaskInitiators);
            builder.Ignore(_ => _.TaskStakeHolders);
            builder.HasMany(_ => _.OperationParameters).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Completions).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.PresentationElements).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.DeadLines).WithOne().OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(_ => _.PeopleAssignments).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.SubTasks).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.EventHistories).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
