using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class LikeCommentMap : StarBerryEntityTypeConfiguration<LikeComment>
    {
        public override void Configure(EntityTypeBuilder<LikeComment> builder)
        {
            base.Configure(builder);
            builder.ToTable("LikeComment");
            builder.Property(t => t.Type).HasColumnName("Type");
            builder.HasOne(c => c.Customer).WithMany(s => s.LikeComments).HasForeignKey(f => f.CustomerId);
            builder.HasOne(c => c.Comment).WithMany(s => s.LikeComments).HasForeignKey(f => f.CommentId);

        }
    }
}