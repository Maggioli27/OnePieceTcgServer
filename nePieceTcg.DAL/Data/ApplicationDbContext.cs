using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using OnePieceTcg.DAL.Configuration;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Collection> Collections { get; set; } = null!;
        public DbSet<CollectionCard> CollectionCards { get; set; } = null!;
        public DbSet<CardType> CardTypes { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Rarity> Rarities { get; set; } = null!;
        public DbSet<CardSet> CardSets { get; set; } = null!;
        public DbSet<Line> Traits { get; set; } = null!;
        public DbSet<CardLine> CardTraits { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CollectionConfiguration());
            modelBuilder.ApplyConfiguration(new CollectionCardConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new RarityConfiguration());
            modelBuilder.ApplyConfiguration(new CardSetConfiguration());
            modelBuilder.ApplyConfiguration(new LineConfiguration());
            modelBuilder.ApplyConfiguration(new CardLineConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialRarityConfiguration());


        }

    }
}
