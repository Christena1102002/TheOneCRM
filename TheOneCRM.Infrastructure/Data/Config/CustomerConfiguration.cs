using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Infrastructure.Data.Config
{
    public class CustomerConfiguration: IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(x => x.CreatedBy)
      .WithMany()
      .HasForeignKey(x => x.CreatedById)
      .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.AssignedTo)
                .WithMany()
                .HasForeignKey(x => x.AssignedToId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.campaigns)
        .WithMany(c => c.Customers)
        .HasForeignKey(c => c.compaignId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

           
        }
    }
}
