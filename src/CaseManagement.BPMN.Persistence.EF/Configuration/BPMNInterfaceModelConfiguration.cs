using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class BPMNInterfaceModelConfiguration : IEntityTypeConfiguration<BPMNInterfaceModel>
    {
        public void Configure(EntityTypeBuilder<BPMNInterfaceModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.HasMany(_ => _.Operations).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
