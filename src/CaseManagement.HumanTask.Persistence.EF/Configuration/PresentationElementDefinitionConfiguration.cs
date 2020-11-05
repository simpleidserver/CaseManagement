using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class PresentationElementDefinitionConfiguration : IEntityTypeConfiguration<PresentationElementDefinition>
    {
        public void Configure(EntityTypeBuilder<PresentationElementDefinition> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        }
    }
}
