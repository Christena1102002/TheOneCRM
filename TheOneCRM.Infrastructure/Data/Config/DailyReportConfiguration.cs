using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Infrastructure.Data.Config
{
    public class DailyReportConfiguration : IEntityTypeConfiguration<DailyReport>
    {
        public void Configure(EntityTypeBuilder<DailyReport> builder)
        {
            builder.ToTable("DailyReports");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReportDate)
                   .IsRequired()
                   .HasColumnType("date");

            builder.Property(x => x.CompletedTasks)
                   .IsRequired()
                   .HasMaxLength(4000);

            builder.Property(x => x.TasksInProgress).HasMaxLength(4000);
            builder.Property(x => x.PlannedTasks).HasMaxLength(4000);
            builder.Property(x => x.Challenges).HasMaxLength(4000);
            builder.Property(x => x.AdditionalNotes).HasMaxLength(2000);

            builder.Property(x => x.WorkHours)
                   .HasColumnType("decimal(5,2)");

            builder.Property(x => x.UserId)
                 .IsRequired();

            // علاقة بالموظف
            builder.HasOne(x => x.appUser)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            //// Index على التاريخ والموظف (موظف واحد ممكن يعمل تقرير لكل يوم)
            //builder.HasIndex(x => new { x.UserId, x.ReportDate })
            //       .IsUnique()
            //       .HasFilter("[IsDeleted] = 0"); // لو عندك soft delete في BaseEntity

            // Index على التاريخ للفلترة السريعة
            //builder.HasIndex(x => x.ReportDate);



        }
    }
}
