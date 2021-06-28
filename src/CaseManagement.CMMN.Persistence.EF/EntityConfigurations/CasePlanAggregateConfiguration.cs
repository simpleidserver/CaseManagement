using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanAggregateConfiguration : IEntityTypeConfiguration<CasePlanAggregate>
    {
        public void Configure(EntityTypeBuilder<CasePlanAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
            builder.HasMany(_ => _.Files).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Roles).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
