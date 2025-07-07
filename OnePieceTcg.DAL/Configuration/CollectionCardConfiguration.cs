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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using OnePieceTcg.Domain.Models;

    public class CollectionCardConfiguration : IEntityTypeConfiguration<CollectionCard>
    {
        public void Configure(EntityTypeBuilder<CollectionCard> builder)
        {
            builder.HasKey(cc => new { cc.UserId, cc.CardId });

            builder.HasOne(cc => cc.User)
                .WithMany(u => u.Collections)
                .HasForeignKey(cc => cc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Card)
                .WithMany(c => c.CollectionCards)
                .HasForeignKey(cc => cc.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
