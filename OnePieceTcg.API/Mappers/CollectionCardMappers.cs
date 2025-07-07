using OnePieceTcg.API.DTOs;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Mappers
{
    public static class CollectionCardMappers
    {
        public static CardInSetDto ToDto(this CollectionCard collectionCard)
        {
            return collectionCard.Card.ToDto();
        }
    }
}
