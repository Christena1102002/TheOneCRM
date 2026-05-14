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
    public class PriceQuotationDetailsConfiguration
        : IEntityTypeConfiguration<PriceQuotationDetails>
    {
        public void Configure(EntityTypeBuilder<PriceQuotationDetails> builder)
        {
            builder.ToTable("PriceQuotationDetails");

            builder.HasOne(d => d.PriceQuotation)
                   .WithMany(p => p.Items)
                   .HasForeignKey(d => d.PriceQuotationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Service)
                   .WithMany()
                   .HasForeignKey(d => d.ServiceId);
        }
    }
}
