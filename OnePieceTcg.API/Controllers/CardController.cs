using Microsoft.AspNetCore.Authorization;
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
    public class CardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("byExtension/{setId}", Name = "GetCardsByExtension")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CardInSetDto>>> GetCardsBySet(int setId)
        {
            var exists = await _context.CardSets.AnyAsync(cs => cs.Id == setId);
            if (!exists)
                return NotFound($"Extension avec l'ID {setId} introuvable.");

            var cards = await _context.Cards
                .Where(c => c.CardSetId == setId)
                .Include(c => c.CardType)
                .Include(c => c.Color)
                .Include(c => c.Rarity)
                .Include(c => c.SpecialRarity)
                .ToListAsync();

            return Ok(cards.Select(c => c.ToDto()));
        }

        [HttpGet("filtered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CardInSetDto>>> GetFilteredCards(
            string? color,
            string? rarity,
            string? type,
            string? specialRarity
        )
        {
            var query = _context.Cards
                .Include(c => c.Color)
                .Include(c => c.Rarity)
                .Include(c => c.CardType)
                .Include(c => c.SpecialRarity)
                .AsQueryable();

            if (!string.IsNullOrEmpty(color))
                query = query.Where(c => c.Color != null && c.Color.Name == color);

            if (!string.IsNullOrEmpty(rarity))
                query = query.Where(c => c.Rarity != null && c.Rarity.Name == rarity);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(c => c.CardType != null && c.CardType.Name == type);

            if (!string.IsNullOrEmpty(specialRarity))
                query = query.Where(c => c.SpecialRarity != null && c.SpecialRarity.Name == specialRarity);

            var cards = await query.ToListAsync();
            return Ok(cards.Select(c => c.ToDto()));
        }






    }
}


