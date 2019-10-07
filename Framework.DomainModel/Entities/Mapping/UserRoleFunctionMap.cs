using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class UserRoleFunctionMap : ProjectNameEntityTypeConfiguration<UserRoleFunction>
    {
        public override void Configure(EntityTypeBuilder<UserRoleFunction> builder)
        {
            base.Configure(builder);
            builder.ToTable("UserRoleFunction");
            builder.Property(t => t.UserRoleId).HasColumnName("UserRoleId");
            builder.Property(t => t.SecurityOperationId).HasColumnName("SecurityOperationId");
            builder.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");

            builder.HasOne(c => c.DocumentType).WithMany(s => s.UserRoleFunctions).HasForeignKey(f => f.DocumentTypeId);
            builder.HasOne(c => c.SecurityOperation).WithMany(s => s.UserRoleFunctions).HasForeignKey(f => f.SecurityOperationId);
            builder.HasOne(c => c.UserRole).WithMany(s => s.UserRoleFunctions).HasForeignKey(f => f.UserRoleId);
        }
    }
}
