using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePieceTcg.Domain.Models
{
    public class Line // trait
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<CardLine> CardLines { get; set; } = new List<CardLine>();
    }
}
