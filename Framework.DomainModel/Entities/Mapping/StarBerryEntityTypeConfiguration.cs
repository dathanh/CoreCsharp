using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class StarBerryEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Id).HasColumnName("Id");
            builder.Property(c => c.CreatedById).HasColumnName("CreatedById");
            builder.Property(c => c.LastUserId).HasColumnName("LastUserId");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(c => c.LastTime).HasColumnName("LastTime");
            builder.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.CreatedById);
            builder.HasOne(t => t.LastUser)
                .WithMany()
                .HasForeignKey(d => d.LastUserId);
        }
    }
}
