namespace OnePieceTcg.API.DTOs
{
    public class CardSetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public DateTime ReleaseDate { get; set; }
        public int CardCount { get; set; }
    }
}
