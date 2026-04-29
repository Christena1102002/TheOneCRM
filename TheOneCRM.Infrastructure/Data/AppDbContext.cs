using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Activities> Activities { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ChatMessagesChannels> chatMessagesChannels { set; get; }
        public  DbSet<Deal> deals { get; set; } 
        public DbSet<Customer>customers { get; set; }
        public DbSet<Notifications> notifications { set; get; }
        public DbSet<PipelineStages> pipelineStages { set; get; }
        public DbSet<SupportTickets> supportTickets { set; get; }
        public DbSet<CustomerServices> customerServices { set; get; }
        public DbSet<Service> Services { set; get; }
        public DbSet<Tasks> tasks { set; get; } 
        public DbSet<Campaigns> campaigns { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<Deal>()
            //    .HasOne(d => d.CreatedBy)
            //     .WithMany()
            //     .HasForeignKey(d => d.CreatedById)
            //     .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Deal>()
            //.HasOne(d => d.AssignedTo)
            //.WithMany()
            //.HasForeignKey(d => d.AssignedToId)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Campaigns>()
    .HasOne(c => c.appUser)
    .WithMany()
    .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
