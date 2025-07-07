using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.DAL.Configuration
{
    public class CardLineConfiguration : IEntityTypeConfiguration<CardLine>
    {
        public void Configure(EntityTypeBuilder<CardLine> builder)
        {
            builder.HasKey(cl => new { cl.CardId, cl.LineId });

            builder.HasOne(cl => cl.Card)
                .WithMany(c => c.CardLines)
                .HasForeignKey(cl => cl.CardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cl => cl.Line)
                .WithMany(l => l.CardLines)
                .HasForeignKey(cl => cl.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
