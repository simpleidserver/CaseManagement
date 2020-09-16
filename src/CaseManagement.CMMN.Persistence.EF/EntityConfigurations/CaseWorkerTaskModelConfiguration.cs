﻿using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseManagement.CMMN.Persistence.EF.EntityConfigurations
{
    public class CaseWorkerTaskModelConfiguration : IEntityTypeConfiguration<CaseWorkerTaskModel>
    {
        public void Configure(EntityTypeBuilder<CaseWorkerTaskModel> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.HasMany(_ => _.Roles).WithOne(_ => _.CaseWorkerTask).HasForeignKey(_ => _.CaseWorkerTaskId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
