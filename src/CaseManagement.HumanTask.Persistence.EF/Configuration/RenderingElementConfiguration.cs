using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class RenderingElementConfiguration : IEntityTypeConfiguration<RenderingElement>
    {
        public void Configure(EntityTypeBuilder<RenderingElement> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasMany(_ => _.Labels).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Values).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
