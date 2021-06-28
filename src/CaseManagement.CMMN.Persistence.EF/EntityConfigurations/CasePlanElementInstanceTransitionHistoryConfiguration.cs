using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanElementInstanceTransitionHistoryConfiguration : IEntityTypeConfiguration<CaseEltInstanceTransitionHistory>
    {
        public void Configure(EntityTypeBuilder<CaseEltInstanceTransitionHistory> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
