using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class NotificationDefinitionConfiguration : IEntityTypeConfiguration<NotificationDefinitionAggregate>
    {
        public void Configure(EntityTypeBuilder<NotificationDefinitionAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.InputParameters);
            builder.Ignore(_ => _.OutputParameters);
            builder.HasMany(_ => _.OperationParameters).WithOne();
            builder.HasMany(_ => _.PeopleAssignments).WithOne();
            builder.HasMany(_ => _.PresentationElements).WithOne();
            builder.HasMany(_ => _.PresentationParameters).WithOne();
        }
    }
}
