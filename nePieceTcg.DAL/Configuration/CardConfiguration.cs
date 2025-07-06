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
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasOne(c => c.CardType)
                .WithMany(ct => ct.Cards)
                .HasForeignKey(c => c.CardTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Color)
                .WithMany(col => col.Cards)
                .HasForeignKey(c => c.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Rarity)
                .WithMany(r => r.Cards)
                .HasForeignKey(c => c.RarityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.CardSet)
                .WithMany(cs => cs.Cards)
                .HasForeignKey(c => c.CardSetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.SpecialRarity)
                .WithMany(sr => sr.Cards)
                .HasForeignKey(c => c.SpecialRarityId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

