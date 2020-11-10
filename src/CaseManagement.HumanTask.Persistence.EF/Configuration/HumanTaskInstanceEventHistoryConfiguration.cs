using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class HumanTaskInstanceEventHistoryConfiguration : IEntityTypeConfiguration<HumanTaskInstanceEventHistory>
    {
        public void Configure(EntityTypeBuilder<HumanTaskInstanceEventHistory> builder)
        {
            builder.HasKey(_ => _.EventId);
        }
    }
}
