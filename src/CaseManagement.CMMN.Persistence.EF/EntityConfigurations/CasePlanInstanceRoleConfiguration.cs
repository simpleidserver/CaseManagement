using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanInstanceRoleConfiguration : IEntityTypeConfiguration<CasePlanInstanceRole>
    {
        public void Configure(EntityTypeBuilder<CasePlanInstanceRole> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
