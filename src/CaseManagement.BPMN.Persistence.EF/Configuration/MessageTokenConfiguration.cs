using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class MessageTokenConfiguration : IEntityTypeConfiguration<MessageToken>
    {
        public void Configure(EntityTypeBuilder<MessageToken> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Ignore(_ => _.JObjMessageContent);
        }
    }
}
