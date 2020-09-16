using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanInstanceModelConfiguration : IEntityTypeConfiguration<CasePlanInstanceModel>
    {
        public void Configure(EntityTypeBuilder<CasePlanInstanceModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasMany(_ => _.Roles).WithOne(_ => _.CasePlanInstance).HasForeignKey(_ => _.CasePlanInstanceId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Files).WithOne(_ => _.CasePlanInstance).HasForeignKey(_ => _.CasePlanInstanceId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.WorkerTasks).WithOne(_ => _.CasePlanInstance).HasForeignKey(_ => _.CasePlanInstanceId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Children).WithOne(_ => _.CasePlanInstance).HasForeignKey(_ => _.CasePlanInstanceId).OnDelete(DeleteBehavior.Cascade);
            builder.Ignore(_ => _.CaseOwner);
        }
    }
}