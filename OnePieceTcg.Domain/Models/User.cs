using System.Xml.Linq;
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
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}
