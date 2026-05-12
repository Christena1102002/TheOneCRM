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
    public class CustomerServicesConfiguration : IEntityTypeConfiguration<CustomerServices>
    {
        public void Configure(EntityTypeBuilder<CustomerServices> builder)
        {
           
            builder.HasKey(x => new { x.customerId, x.ServiceId });



            builder.HasOne(cs => cs.customers)
                   .WithMany(c => c.customerServices)
                   .HasForeignKey(cs => cs.customerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.Service)
                   .WithMany(s => s.customerServices)
                   .HasForeignKey(cs => cs.ServiceId)
                   .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(x => x.Service)
            //    .WithMany(x => x.customerServices)
            //    .HasForeignKey(x => x.ServiceId)
            //    .OnDelete(DeleteBehavior.NoAction);

      //      builder.HasOne(cs => cs.customers)
      //.WithMany(c => c.customerServices)
      //.HasForeignKey(cs => cs.customerId)
      //.OnDelete(DeleteBehavior.Cascade);


        }
    }
}
