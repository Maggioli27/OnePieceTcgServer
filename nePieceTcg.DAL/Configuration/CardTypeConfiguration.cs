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
    public class CardTypeConfiguration : IEntityTypeConfiguration<CardType>
    {
        public void Configure(EntityTypeBuilder<CardType> builder)
        {
            builder.HasMany(ct => ct.Cards)
                .WithOne(c => c.CardType)
                .HasForeignKey(c => c.CardTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
