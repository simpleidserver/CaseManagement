using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.BPMN.Persistence.EF.Configuration
{
    public class StateTransitionTokenConfiguration : IEntityTypeConfiguration<StateTransitionToken>
    {
        public void Configure(EntityTypeBuilder<StateTransitionToken> builder)
        {
            builder.Property<int>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Ignore(_ => _.JObjContent);
        }
    }
}
