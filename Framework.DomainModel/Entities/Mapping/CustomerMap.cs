using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CustomerMap : StarBerryEntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Customer");
            builder.Property(t => t.UserName).HasColumnName("UserName");
            builder.Property(t => t.Password).HasColumnName("Password");
            builder.Property(t => t.Dob).HasColumnName("DOB");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.Gender).HasColumnName("Gender");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.FullName).HasColumnName("FullName");
            builder.Property(t => t.Phone).HasColumnName("Phone");
            builder.Property(t => t.Email).HasColumnName("Email");
            builder.Property(t => t.Avatar).HasColumnName("Avatar");
            builder.Property(t => t.CategoryConfig).HasColumnName("CategoryConfig");
            builder.Property(t => t.IsAccountFacebook).HasColumnName("IsAccountFacebook");
            builder.Property(t => t.IsAccountGoogle).HasColumnName("IsAccountGoogle");
            builder.Property(t => t.IsCompleteSetupCategory).HasColumnName("IsCompleteSetupCategory");
            builder.Property(t => t.LanguageId).HasColumnName("LanguageId");
            builder.Property(t => t.ActiveCode).HasColumnName("ActiveCode");
            builder.Property(t => t.ExpiredTime).HasColumnName("ExpiredTime");
            builder.Property(t => t.ForgotPassword).HasColumnName("ForgotPassword");
            builder.Property(t => t.UnsubscribeCode).HasColumnName("UnsubscribeCode");
            builder.Property(t => t.IsUnsubscribe).HasColumnName("IsUnsubscribe");
            builder.HasOne(c => c.Language).WithMany(s => s.Customers).HasForeignKey(f => f.LanguageId);
        }
    }
}