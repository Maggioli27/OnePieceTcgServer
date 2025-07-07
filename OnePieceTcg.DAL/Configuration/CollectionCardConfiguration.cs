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
    public class CollectionCardConfiguration : IEntityTypeConfiguration<CollectionCard>
    {
        public void Configure(EntityTypeBuilder<CollectionCard> builder)
        {
            builder.HasKey(cc => new { cc.CollectionId, cc.CardId });

            builder.HasOne(cc => cc.Collection)
                .WithMany(c => c.CollectionCards)
                .HasForeignKey(cc => cc.CollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cc => cc.Card)
                .WithMany(c => c.CollectionCards)
                .HasForeignKey(cc => cc.CardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
