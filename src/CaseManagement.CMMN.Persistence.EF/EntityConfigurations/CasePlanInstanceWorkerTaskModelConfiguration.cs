using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanInstanceWorkerTaskModelConfiguration : IEntityTypeConfiguration<CasePlanInstanceWorkerTaskModel>
    {
        public void Configure(EntityTypeBuilder<CasePlanInstanceWorkerTaskModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();
        }
    }
}
