using CaseManagement.CMMN.Domains.CasePlanInstance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CaseEltInstancePropertyConfiguration : IEntityTypeConfiguration<CaseEltInstanceProperty>
    {
        public void Configure(EntityTypeBuilder<CaseEltInstanceProperty> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
