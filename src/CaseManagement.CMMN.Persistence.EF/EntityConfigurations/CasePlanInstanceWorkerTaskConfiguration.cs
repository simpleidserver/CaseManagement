using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CasePlanInstanceWorkerTaskConfiguration : IEntityTypeConfiguration<CasePlanInstanceWorkerTask>
    {
        public void Configure(EntityTypeBuilder<CasePlanInstanceWorkerTask> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
