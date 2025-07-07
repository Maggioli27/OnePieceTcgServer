using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class CollectionCard
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CardId { get; set; }
        public Card Card { get; set; } = null!;
    }
}
