using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class PeopleAssignmentInstanceConfiguration : IEntityTypeConfiguration<PeopleAssignmentInstance>
    {
        public void Configure(EntityTypeBuilder<PeopleAssignmentInstance> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        }
    }
}
