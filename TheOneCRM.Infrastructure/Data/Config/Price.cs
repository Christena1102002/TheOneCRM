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
    public class Price : IEntityTypeConfiguration<PriceQuotation>
    {
        public void Configure(EntityTypeBuilder<PriceQuotation> builder)
        {
            builder.ToTable("PriceQuotations");

            builder.HasMany(p => p.Items)
                   .WithOne(d => d.PriceQuotation)
                   .HasForeignKey(d => d.PriceQuotationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
