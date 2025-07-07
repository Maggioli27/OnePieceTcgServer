using OnePieceTcg.API.DTOs;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Mappers
{
    public static class CardMappers
    {
        public static CardInSetDto ToDto(this Card card)
        {
            return new CardInSetDto
            {
                Id = card.Id,
                Name = card.Name,
                Type = card.CardType?.Name ?? string.Empty,
                Color = card.Color?.Name ?? string.Empty,
                Rarity = card.Rarity?.Name ?? string.Empty,
                SpecialRarity = card.SpecialRarity?.Name ?? string.Empty,
                ImageUrl = card.ImageUrl,
                Series = card.Series ?? string.Empty
            };
        }
    }
}
