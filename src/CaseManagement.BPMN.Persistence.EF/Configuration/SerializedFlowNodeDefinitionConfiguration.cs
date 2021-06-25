using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class SerializedFlowNodeDefinitionConfiguration : IEntityTypeConfiguration<SerializedFlowNodeDefinition>
    {
        public void Configure(EntityTypeBuilder<SerializedFlowNodeDefinition> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
