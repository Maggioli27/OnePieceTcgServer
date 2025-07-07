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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPost(Name = "CreateCard")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCard([FromBody] CardCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cardSet = await _context.CardSets.SingleOrDefaultAsync(cs => cs.Name == dto.CardSetName);
            if (cardSet == null)
                return BadRequest($"Extension '{dto.CardSetName}' introuvable.");


            var color = string.IsNullOrEmpty(dto.ColorName) ? null :
                await _context.Colors.SingleOrDefaultAsync(c => c.Name == dto.ColorName);

            var rarity = string.IsNullOrEmpty(dto.RarityName) ? null :
                await _context.Rarities.SingleOrDefaultAsync(r => r.Name == dto.RarityName);

            var specialRarity = string.IsNullOrEmpty(dto.SpecialRarityName) ? null :
                await _context.SpecialRarities.SingleOrDefaultAsync(sr => sr.Name == dto.SpecialRarityName);

            var card = new Card
            {
                Name = dto.Name,
                Series = dto.Series,
                ImageUrl = dto.ImageUrl,
                CardSetId = cardSet.Id,
                ColorId = color?.Id,
                RarityId = rarity?.Id,
                SpecialRarityId = specialRarity?.Id
            };

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCardsBySet), new { setId = card.CardSetId }, new
            {
                message = "Carte créée avec succès",
                id = card.Id
            });
        }
        [HttpDelete("{id}", Name = "DeleteCard")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
                return NotFound($"Aucune carte trouvée avec l'ID {id}.");

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Carte supprimée avec succès", id = card.Id, name = card.Name });
        }


    }
}

