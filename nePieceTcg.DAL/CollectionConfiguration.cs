using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.DAL
{
    public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.User)
                   .WithMany(u => u.Collections)
                   .HasForeignKey(c => c.UserId);

            builder.HasMany(c => c.CollectionCards)
                   .WithOne(cc => cc.Collection)
                   .HasForeignKey(cc => cc.CollectionId);
        }
    }
}
