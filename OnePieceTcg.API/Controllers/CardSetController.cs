using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePieceTcg.API.DTOs;
using OnePieceTcg.API.Mappers;
using OnePieceTcg.DAL.Data;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardSetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CardSetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "Collections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CardSetDto>>> GetAllCardSets()
        {
            var sets = await _context.CardSets.ToListAsync();
            var setDtos = sets.Select(CardSetMappers.ToDto).ToList();
            return Ok(setDtos);
        }

        [HttpGet("{setId}/cards", Name = "Cartes dans la collection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CardInSetDto>>> GetCardsInSet(int setId)
        {
            var exists = await _context.CardSets.AnyAsync(cs => cs.Id == setId);
            if (!exists)
                return NotFound($"Le set avec l'ID {setId} n'existe pas.");

            var cards = await _context.Cards
                .Where(c => c.CardSetId == setId)
                .Include(c => c.CardType)
                .Include(c => c.Color)
                .Include(c => c.Rarity)
                .Include(c => c.SpecialRarity)
                .ToListAsync();

            var cardDtos = cards.Select(CardSetMappers.ToDto).ToList();

            return Ok(cardDtos);
        }
    }

}
