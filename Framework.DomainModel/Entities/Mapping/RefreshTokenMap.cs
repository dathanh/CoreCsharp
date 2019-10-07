﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class RefreshTokenMap : StarBerryEntityTypeConfiguration<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);
            builder.ToTable("RefreshToken");
            builder.Property(t => t.Token).HasColumnName("Token");
            builder.Property(t => t.Expires).HasColumnName("Expires");
            builder.Property(t => t.CustomerId).HasColumnName("CustomerId");
            builder.HasOne(c => c.Customer).WithMany(s => s.RefreshTokens).HasForeignKey(f => f.CustomerId);
        }
    }
}