using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanFileItemConfiguration : IEntityTypeConfiguration<CasePlanFileItem>
    {
        public void Configure(EntityTypeBuilder<CasePlanFileItem> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}