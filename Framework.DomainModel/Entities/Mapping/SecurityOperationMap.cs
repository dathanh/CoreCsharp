using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class SecurityOperationMap : ProjectNameEntityTypeConfiguration<SecurityOperation>
    {
        public override void Configure(EntityTypeBuilder<SecurityOperation> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.ToTable("SecurityOperation");
            builder.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
