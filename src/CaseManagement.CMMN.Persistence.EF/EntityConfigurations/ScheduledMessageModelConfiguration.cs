using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class ScheduledMessageModelConfiguration : IEntityTypeConfiguration<ScheduledMessageModel>
    {
        public void Configure(EntityTypeBuilder<ScheduledMessageModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();
        }
    }
}
