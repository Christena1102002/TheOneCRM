using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Infrastructure.Data.Config
{
    public class CustomerActivityConfiguration : IEntityTypeConfiguration<CustomerActivity>
    {
        public void Configure(EntityTypeBuilder<CustomerActivity> builder)
        {
            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.CustomerId, a.ActivityType });
            builder.HasIndex(a => a.CreatedAt);
        }
    }
}
