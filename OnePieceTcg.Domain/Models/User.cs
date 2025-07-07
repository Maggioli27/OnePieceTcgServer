using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePieceTcg.Domain.Enum;

namespace OnePieceTcg.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }

        public string Password { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;

        public ICollection<Card> cards { get; set; } = new List<Card>();

        public ICollection<CollectionCard> Collections { get; set; } = new List<CollectionCard>();
        public ICollection<FavoriteCard> FavoriteCards { get; set; } = new List<FavoriteCard>();

    }
}
