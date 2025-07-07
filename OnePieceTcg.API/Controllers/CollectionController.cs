using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePieceTcg.API.DTOs;
using OnePieceTcg.API.Helpers;
using OnePieceTcg.API.Mappers;
using OnePieceTcg.DAL.Data;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("own")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkCardAsOwned([FromBody] int cardId)
        {
            int userId = User.GetUserId();

            var exists = await _context.Cards.AnyAsync(c => c.Id == cardId);
            if (!exists)
                return NotFound("Carte non trouvée.");

            var alreadyOwned = await _context.CollectionCards
                .AnyAsync(cc => cc.UserId == userId && cc.CardId == cardId);

            if (alreadyOwned)
                return BadRequest("Carte déjà possédée.");

            _context.CollectionCards.Add(new CollectionCard
            {
                UserId = userId,
                CardId = cardId
            });

            await _context.SaveChangesAsync();

            return Ok(new { message = "Carte marquée comme possédée." });
        }
        [HttpDelete("own/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnmarkCardAsOwned(int cardId)
        {
            int userId = User.GetUserId();

            var entry = await _context.CollectionCards
                .SingleOrDefaultAsync(cc => cc.UserId == userId && cc.CardId == cardId);

            if (entry == null)
                return NotFound("Carte non cochée.");

            _context.CollectionCards.Remove(entry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Carte décochée avec succès." });
        }
        [HttpGet("owned")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CardInSetDto>>> GetOwnedCards()
        {
            int userId = User.GetUserId();

            var cards = await _context.CollectionCards
                .Where(cc => cc.UserId == userId)
                .Select(cc => cc.Card)
                .Include(c => c.CardType)
                .Include(c => c.Color)
                .Include(c => c.Rarity)
                .Include(c => c.SpecialRarity)
                .ToListAsync();

            return Ok(cards.Select(c => c.ToDto()));
        }

        [HttpGet("stats/completion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCompletionStatsPerExtension()
        {
            int userId = User.GetUserId();

            var totalBySet = await _context.Cards
                .GroupBy(c => c.CardSet.Name)
                .Select(g => new
                {
                    CardSetName = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            var ownedBySet = await _context.CollectionCards
                .Where(cc => cc.UserId == userId)
                .Include(cc => cc.Card)
                .GroupBy(cc => cc.Card.CardSet.Name)
                .Select(g => new
                {
                    CardSetName = g.Key,
                    Owned = g.Count()
                })
                .ToListAsync();

            var stats = totalBySet
                .Select(total =>
                {
                    var owned = ownedBySet.FirstOrDefault(o => o.CardSetName == total.CardSetName)?.Owned ?? 0;
                    var percent = total.Total == 0 ? 0 : (owned * 100.0 / total.Total);

                    return new
                    {
                        Extension = total.CardSetName,
                        Total = total.Total,
                        Possessed = owned,
                        Completion = Math.Round(percent, 2)
                    };
                })
                .OrderByDescending(s => s.Completion)
                .ToList();

            return Ok(stats);
        }


    }
}

