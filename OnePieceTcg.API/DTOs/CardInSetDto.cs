namespace OnePieceTcg.API.DTOs
{
    public class CardInSetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Color { get; set; } = "";
        public string Rarity { get; set; } = "";
        public string SpecialRarity { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Series { get; set; } = "";
    }
}
