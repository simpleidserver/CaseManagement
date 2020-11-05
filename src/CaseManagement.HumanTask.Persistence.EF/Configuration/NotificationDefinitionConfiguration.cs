using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class NotificationDefinitionConfiguration : IEntityTypeConfiguration<NotificationDefinition>
    {
        public void Configure(EntityTypeBuilder<NotificationDefinition> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.Ignore(_ => _.InputParameters);
            builder.Ignore(_ => _.OutputParameters);
            builder.HasMany(_ => _.OperationParameters).WithOne();
            builder.HasMany(_ => _.PeopleAssignments).WithOne();
            builder.HasMany(_ => _.PresentationElements).WithOne();
            builder.HasMany(_ => _.PresentationParameters).WithOne();
        }
    }
}
