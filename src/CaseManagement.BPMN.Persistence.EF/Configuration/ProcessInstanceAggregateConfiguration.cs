using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ProcessInstanceAggregateConfiguration : IEntityTypeConfiguration<ProcessInstanceAggregate>
    {
        public void Configure(EntityTypeBuilder<ProcessInstanceAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.DomainEvents);
            builder.Ignore(_ => _.StartEvts);
            builder.Ignore(_ => _.ElementDefs);
            builder.HasMany(_ => _.ItemDefs).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Interfaces).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Messages).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.SequenceFlows).WithOne().OnDelete(DeleteBehavior.Cascade);
            /*
            builder.HasMany(_ => _.ElementDefs).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.SequenceFlows).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.ElementInstances).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.ExecutionPathLst).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.StateTransitions).WithOne().OnDelete(DeleteBehavior.Cascade);
            */
        }
    }
}
