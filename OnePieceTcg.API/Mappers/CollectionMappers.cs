using OnePieceTcg.API.DTOs;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Mappers
{
    public static class CollectionMappers
    {
        public static CollectionDto ToDto(Collection collection)
        {
            return new CollectionDto
            {
                Id = collection.Id,
                Name = collection.Name,
                UserId = collection.UserId
            };
        }
    }
}
