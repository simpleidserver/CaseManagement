using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskDefinitionConfiguration : IEntityTypeConfiguration<HumanTaskDefinitionAggregate>
    {
        public void Configure(EntityTypeBuilder<HumanTaskDefinitionAggregate> builder)
        {
            builder.HasKey(_ => _.AggregateId);
            builder.Ignore(_ => _.InputParameters);
            builder.Ignore(_ => _.OutputParameters);
            builder.HasMany(_ => _.Completions).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.OperationParameters).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.SubTasks).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.RenderingElements).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.DeadLines).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.PeopleAssignments).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.PresentationElements).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.PresentationParameters).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.CallbackOperations).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
