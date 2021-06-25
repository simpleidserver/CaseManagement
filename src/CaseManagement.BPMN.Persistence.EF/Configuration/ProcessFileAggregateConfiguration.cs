using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ProcessFileAggregateConfiguration : IEntityTypeConfiguration<ProcessFileAggregate>
    {
        public void Configure(EntityTypeBuilder<ProcessFileAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
        }
    }
}
