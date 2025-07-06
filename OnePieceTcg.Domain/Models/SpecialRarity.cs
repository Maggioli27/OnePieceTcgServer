using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class SpecialRarity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relation inverse
        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
