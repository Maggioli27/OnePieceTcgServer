using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class CollectionCard
    {
        public int CollectionId { get; set; }
        public Collection Collection { get; set; } = null!;

        public int CardId { get; set; }
        public Card Card { get; set; } = null!;

        public int Quantity { get; set; } = 1;
    }
}
