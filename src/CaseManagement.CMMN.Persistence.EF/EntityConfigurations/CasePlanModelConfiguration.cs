using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanModelConfiguration : IEntityTypeConfiguration<CasePlanModel>
    {
        public void Configure(EntityTypeBuilder<CasePlanModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasMany(_ => _.Roles).WithOne(_ => _.CasePlan).HasForeignKey(_ => _.CasePlanId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Files).WithOne(_ => _.CasePlan).HasForeignKey(_ => _.CasePlanId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
