using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class CardType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
