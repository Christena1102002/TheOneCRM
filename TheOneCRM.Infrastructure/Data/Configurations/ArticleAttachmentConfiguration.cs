using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Infrastructure.Data.Configurations
{
    public class ArticleAttachmentConfiguration : IEntityTypeConfiguration<ArticleAttachment>
    {
        public void Configure(EntityTypeBuilder<ArticleAttachment> builder)
        {
            builder.HasOne(a => a.Article)
                .WithMany(ar => ar.Attachments)
                .HasForeignKey(a => a.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
