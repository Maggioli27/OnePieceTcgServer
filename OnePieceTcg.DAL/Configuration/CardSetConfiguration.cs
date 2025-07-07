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
    public class CardSetConfiguration : IEntityTypeConfiguration<CardSet>
    {
        public void Configure(EntityTypeBuilder<CardSet> builder)
        {
            builder.HasMany(cs => cs.Cards)
                .WithOne(c => c.CardSet)
                .HasForeignKey(c => c.CardSetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
