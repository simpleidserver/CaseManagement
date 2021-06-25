using CaseManagement.BPMN.Domains.DelegateConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class DelegateConfigurationRecordConfiguration : IEntityTypeConfiguration<DelegateConfigurationRecord>
    {
        public void Configure(EntityTypeBuilder<DelegateConfigurationRecord> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
