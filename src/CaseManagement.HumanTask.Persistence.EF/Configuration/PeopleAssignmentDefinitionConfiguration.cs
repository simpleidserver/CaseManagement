using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CaseManagement.HumanTask.Persistence.EF.Configuration
{
    public class PeopleAssignmentDefinitionConfiguration : IEntityTypeConfiguration<PeopleAssignmentDefinition>
    {
        public void Configure(EntityTypeBuilder<PeopleAssignmentDefinition> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        }
    }
}
