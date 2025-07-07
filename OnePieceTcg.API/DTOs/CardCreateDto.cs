namespace OnePieceTcg.API.DTOs
{
    public class CardCreateDto
    {
        public string Name { get; set; } = null!;
        public string Series { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string CardSetName { get; set; } = null!;
        public string? ColorName { get; set; }
        public string? RarityName { get; set; }
        public string? SpecialRarityName { get; set; }
    }

}
