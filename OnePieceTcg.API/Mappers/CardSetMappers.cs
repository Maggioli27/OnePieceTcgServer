using OnePieceTcg.API.DTOs;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Mappers
{
    public static class CardSetMappers
    {

        public static CardSetDto ToDto(CardSet set)
        {
            return new CardSetDto
            {
                Id = set.Id,
                Name = set.Name,
                Code = set.Code,
            };
        }
    }
}
