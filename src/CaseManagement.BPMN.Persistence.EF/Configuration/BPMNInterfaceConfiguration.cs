using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class BPMNInterfaceConfiguration : IEntityTypeConfiguration<BPMNInterface>
    {
        public void Configure(EntityTypeBuilder<BPMNInterface> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.HasMany(_ => _.Operations).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
