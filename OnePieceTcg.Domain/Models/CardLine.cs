using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class CardLine
    {
        public int CardId { get; set; }
        public Card Card { get; set; } = null!;

        public int LineId { get; set; }
        public Line Line { get; set; } = null!;
    }
}
