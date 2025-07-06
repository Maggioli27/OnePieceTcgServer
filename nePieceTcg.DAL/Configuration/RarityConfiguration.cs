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
    public class RarityConfiguration : IEntityTypeConfiguration<Rarity>
    {
        public void Configure(EntityTypeBuilder<Rarity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasMany(r => r.Cards)
                   .WithOne(c => c.Rarity)
                   .HasForeignKey(c => c.RarityId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
