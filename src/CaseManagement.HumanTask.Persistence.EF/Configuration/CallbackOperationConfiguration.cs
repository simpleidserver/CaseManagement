using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class CallbackOperationConfiguration : IEntityTypeConfiguration<CallbackOperation>
    {
        public void Configure(EntityTypeBuilder<CallbackOperation> builder)
        {
            builder.HasKey(_ => _.Id);
        }
    }
}
