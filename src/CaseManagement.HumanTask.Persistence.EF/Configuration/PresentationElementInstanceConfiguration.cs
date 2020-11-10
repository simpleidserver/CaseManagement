using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class PresentationElementInstanceConfiguration : IEntityTypeConfiguration<PresentationElementInstance>
    {
        public void Configure(EntityTypeBuilder<PresentationElementInstance> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        }
    }
}
