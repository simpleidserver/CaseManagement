using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ProcessInstanceModelConfiguration : IEntityTypeConfiguration<ProcessInstanceModel>
    {
        public void Configure(EntityTypeBuilder<ProcessInstanceModel> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.HasMany(_ => _.ItemDefs).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Interfaces).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Messages).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.ElementDefs).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.SequenceFlows).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.ElementInstances).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.ExecutionPathLst).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.StateTransitions).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
