using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ActivityStateHistoryConfiguration : IEntityTypeConfiguration<ActivityStateHistory>
    {
        public void Configure(EntityTypeBuilder<ActivityStateHistory> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
