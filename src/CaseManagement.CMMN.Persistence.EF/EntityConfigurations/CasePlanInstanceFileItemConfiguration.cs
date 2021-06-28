using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanInstanceFileItemConfiguration : IEntityTypeConfiguration<CasePlanInstanceFileItem>
    {
        public void Configure(EntityTypeBuilder<CasePlanInstanceFileItem> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
