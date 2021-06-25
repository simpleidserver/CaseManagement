using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class BPMNTranslationConfiguration : IEntityTypeConfiguration<BPMNTranslation>
    {
        public void Configure(EntityTypeBuilder<BPMNTranslation> builder)
        {
            builder.HasKey(_ => _.Key);
        }
    }
}
