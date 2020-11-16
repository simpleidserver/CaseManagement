using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class ProcessFileModelConfiguration : IEntityTypeConfiguration<ProcessFileModel>
    {
        public void Configure(EntityTypeBuilder<ProcessFileModel> builder)
        {
            builder.HasKey(_ => _.Id);
        }
    }
}
