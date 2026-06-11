using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Infrastructure.Data.Configurations
{
    public class GoalCompletionConfiguration : IEntityTypeConfiguration<GoalCompletion>
    {
        public void Configure(EntityTypeBuilder<GoalCompletion> builder)
        {
            builder.HasOne(gc => gc.Goal)
                .WithMany(g => g.Completions)
                .HasForeignKey(gc => gc.GoalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gc => gc.User)
                .WithMany()
                .HasForeignKey(gc => gc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(gc => gc.UserId);
            builder.HasIndex(gc => gc.GoalId);
        }
    }
}
