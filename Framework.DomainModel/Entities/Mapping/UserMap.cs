using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class UserMap : StarBerryEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("User");
            builder.Property(t => t.UserName).HasColumnName("UserName");
            builder.Property(t => t.IsSystemUser).HasColumnName("IsSystemUser");
            builder.Property(t => t.Password).HasColumnName("Password");
            builder.Property(t => t.UserRoleId).HasColumnName("UserRoleId");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.FullName).HasColumnName("FullName");
            builder.Property(t => t.Phone).HasColumnName("Phone");
            builder.Property(t => t.Email).HasColumnName("Email");
            builder.Property(t => t.Avatar).HasColumnName("Avatar");
            builder.Property(t => t.IsAccountFacebook).HasColumnName("IsAccountFacebook");
            builder.Property(t => t.IsAccountGoogle).HasColumnName("IsAccountGoogle");
            builder.Property(t => t.Passport).HasColumnName("Passport");
            builder.Property(t => t.PassportImageId).HasColumnName("PassportImageId");
            builder.Property(t => t.StatusId).HasColumnName("StatusId");
            builder.HasOne(c => c.UserRole).WithMany(s => s.Users).HasForeignKey(f => f.UserRoleId);
        }
    }
}
