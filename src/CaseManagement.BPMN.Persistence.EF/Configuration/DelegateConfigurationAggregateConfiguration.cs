using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class DelegateConfigurationAggregateConfiguration : IEntityTypeConfiguration<DelegateConfigurationAggregate>
    {
        public void Configure(EntityTypeBuilder<DelegateConfigurationAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DisplayNames);
            builder.Ignore(_ => _.Descriptions);
            builder.Ignore(_ => _.DomainEvents);
            builder.HasMany(c => c.Translations).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Records).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
