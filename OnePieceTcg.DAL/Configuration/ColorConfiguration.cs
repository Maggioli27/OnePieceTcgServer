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
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Cards)
                   .WithOne(card => card.Color)
                   .HasForeignKey(card => card.ColorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
