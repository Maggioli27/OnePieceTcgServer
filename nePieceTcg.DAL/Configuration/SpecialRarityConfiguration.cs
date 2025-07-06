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
    public class SpecialRarityConfiguration : IEntityTypeConfiguration<SpecialRarity>
    {
        public void Configure(EntityTypeBuilder<SpecialRarity> builder)
        {
            builder.HasKey(sr => sr.Id);
            builder.Property(sr => sr.Name).IsRequired();
        }
    }

}
