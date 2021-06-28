using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CaseWorkerTaskAggregateConfiguration : IEntityTypeConfiguration<CaseWorkerTaskAggregate>
    {
        public void Configure(EntityTypeBuilder<CaseWorkerTaskAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
        }
    }
}
