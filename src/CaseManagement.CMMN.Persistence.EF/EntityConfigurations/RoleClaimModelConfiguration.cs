using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class RoleClaimModelConfiguration : IEntityTypeConfiguration<RoleClaimModel>
    {
        public void Configure(EntityTypeBuilder<RoleClaimModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();
        }
    }
}
