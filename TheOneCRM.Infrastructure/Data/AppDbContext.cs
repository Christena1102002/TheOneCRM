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

        public DbSet<DailyReport> dailyReports { get; set; }
        //public DbSet<Activities> Activities { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Projects> Project { get; set; }
        public DbSet<ProjectEngineer> ProjectEngineers { get; set; }
        public DbSet<ChatMessagesChannels> chatMessagesChannels { set; get; }
        public  DbSet<Deal> deals { get; set; } 
        public DbSet<Customer>customers { get; set; }
        public DbSet<Notifications> notifications { set; get; }
        public DbSet<PipelineStages> pipelineStages { set; get; }
        public DbSet<SupportTickets> supportTickets { set; get; }
        public DbSet<CustomerServices> customerServices { set; get; }
        public DbSet<Service> Services { set; get; }
        public DbSet<Tasks> tasks { set; get; }
        public DbSet<TaskAssignee> TaskAssignees { get; set; }
        public DbSet<ChannelSource> channelSources { set; get; }
        public DbSet<Campaigns> campaigns { set; get; }
        public DbSet<PriceQuotation> priceQuotations { set; get; }
        public DbSet<PriceQuotationDetails> priceQuotationDetails { set; get; }
        public DbSet<CustomerNote> CustomerNotes { get; set; }
        public DbSet<CustomerAssignmentHistory> CustomerAssignmentHistories { get; set; }
        public DbSet<CustomerActivity> CustomerActivities { get; set; }
       public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleAttachment> ArticleAttachments { get; set; }
        public DbSet<CompanySettings> CompanySettings { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalCompletion> GoalCompletions { get; set; }
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

            // SupportTickets: امنع تعدد مسارات الـ cascade
            modelBuilder.Entity<SupportTickets>()
                .HasOne(t => t.Customer)
                .WithMany()
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupportTickets>()
                .HasOne(t => t.Service)
                .WithMany()
                .HasForeignKey(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Projects: امنع تعدد مسارات الـ cascade على AspNetUsers و Customer
            modelBuilder.Entity<Projects>()
                .HasOne(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Projects>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjectEngineer: many-to-many بين المشروع والمهندسين
            modelBuilder.Entity<ProjectEngineer>()
                .HasKey(pe => new { pe.ProjectId, pe.EngineerId });

            modelBuilder.Entity<ProjectEngineer>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEngineers)
                .HasForeignKey(pe => pe.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectEngineer>()
                .HasOne(pe => pe.Engineer)
                .WithMany()
                .HasForeignKey(pe => pe.EngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tasks: امنع تعدد مسارات الـ cascade على AspNetUsers و Projects
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            // Article: امنع cascade على AspNetUsers
            modelBuilder.Entity<Article>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ArticleAttachment: المرفقات تتمسح مع المقالة
            modelBuilder.Entity<ArticleAttachment>()
                .HasOne(att => att.Article)
                .WithMany(a => a.Attachments)
                .HasForeignKey(att => att.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // PriceQuotation: امنع cascade على AspNetUsers
            modelBuilder.Entity<PriceQuotation>()
                .HasOne(q => q.CreatedBy)
                .WithMany()
                .HasForeignKey(q => q.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Notifications: ربط بالمستخدم
            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notifications>()
                .HasIndex(n => new { n.UserId, n.IsRead });

        }
    }
}
