using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CaseFileAggregateConfiguration : IEntityTypeConfiguration<CaseFileAggregate>
    {
        public void Configure(EntityTypeBuilder<CaseFileAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
        }
    }
}