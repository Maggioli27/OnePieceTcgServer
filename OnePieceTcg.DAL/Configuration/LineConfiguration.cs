using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.DAL.Configuration
{
    public class LineConfiguration : IEntityTypeConfiguration<Line>
    {
        public void Configure(EntityTypeBuilder<Line> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasMany(t => t.CardLines)
                   .WithOne(cl => cl.Line)
                   .HasForeignKey(cl => cl.LineId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
