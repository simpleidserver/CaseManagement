using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class NotificationInstanceAggregateConfiguration : IEntityTypeConfiguration<NotificationInstanceAggregate>
    {
        public void Configure(EntityTypeBuilder<NotificationInstanceAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
            builder.Ignore(_ => _.Names);
            builder.Ignore(_ => _.Descriptions);
            builder.Ignore(_ => _.Subjects);
            builder.Property(_ => _.OperationParameters).HasConversion(v => JsonConvert.SerializeObject(v),
                v => v == null ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            builder.HasMany(_ => _.PresentationElements).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.PeopleAssignments).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
