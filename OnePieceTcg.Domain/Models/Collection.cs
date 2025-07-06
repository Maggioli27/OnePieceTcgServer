using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<CollectionCard> CollectionCards { get; set; } = new List<CollectionCard>();

    }
}
