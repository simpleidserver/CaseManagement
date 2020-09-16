using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanElementInstanceModelConfiguration : IEntityTypeConfiguration<CasePlanElementInstanceModel>
    {
        public void Configure(EntityTypeBuilder<CasePlanElementInstanceModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();
            builder.HasMany(_ => _.Children).WithOne(_ => _.Parent).HasForeignKey(_ => _.ParentId);
        }
    }
}
