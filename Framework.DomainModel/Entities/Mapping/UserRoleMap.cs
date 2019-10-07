using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class UserRoleMap : ProjectNameEntityTypeConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.ToTable("UserRole");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.AppRoleName).HasColumnName("AppRoleName");
        }
    }
}
