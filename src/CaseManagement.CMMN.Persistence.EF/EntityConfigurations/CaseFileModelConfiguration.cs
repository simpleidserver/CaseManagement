using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CaseFileModelConfiguration : IEntityTypeConfiguration<CaseFileModel>
    {
        public void Configure(EntityTypeBuilder<CaseFileModel> builder)
        {
            builder.HasKey(_ => _.Id);
        }
    }
}