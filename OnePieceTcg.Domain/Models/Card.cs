using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Series { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int Cost { get; set; }
        public string? Description { get; set; }




        // Remplace les string par des FK et navigation vers entités liées
        public int? CardTypeId { get; set; }
        public CardType? CardType { get; set; }

        public int? ColorId { get; set; }
        public Color? Color { get; set; }

        public int? RarityId { get; set; }
        public Rarity? Rarity { get; set; }

        public int? CardSetId { get; set; }
        public CardSet? CardSet { get; set; }
        public int? SpecialRarityId { get; set; }
        public SpecialRarity? SpecialRarity { get; set; }

        // Relations many-to-many via CollectionCard
        public ICollection<CollectionCard> CollectionCards { get; set; } = new List<CollectionCard>();

        // Relations many-to-many avec Traits (Lignes)
        public ICollection<CardLine> CardLines { get; set; } = new List<CardLine>();
    }
}
