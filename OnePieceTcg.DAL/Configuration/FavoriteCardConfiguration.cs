using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.DAL.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using OnePieceTcg.Domain.Models;

    public class FavoriteCardConfiguration : IEntityTypeConfiguration<FavoriteCard>
    {
        public void Configure(EntityTypeBuilder<FavoriteCard> builder)
        {

            builder.HasKey(fc => new { fc.UserId, fc.CardId });


            builder.HasOne(fc => fc.User)
                   .WithMany(u => u.FavoriteCards)
                   .HasForeignKey(fc => fc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(fc => fc.Card)
                   .WithMany()
                   .HasForeignKey(fc => fc.CardId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
