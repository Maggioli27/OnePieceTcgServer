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
    [Route("api/user/collection")]
    [Authorize]
    public class UserCollectionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserCollectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CardInSetDto>>> GetMyCollection()
        {
            int userId = User.GetUserId();

            var collectionCards = await _context.CollectionCards
                .Where(cc => cc.UserId == userId)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.CardType)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.Color)
                .ToListAsync();

            var result = collectionCards.Select(CollectionCardMappers.ToDto);
            return Ok(result);
        }

        [HttpPost("add/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCardToCollection(int cardId)
        {
            int userId = User.GetUserId();

            if (!await _context.Cards.AnyAsync(c => c.Id == cardId))
                return NotFound("Carte introuvable.");

            bool exists = await _context.CollectionCards.AnyAsync(cc => cc.UserId == userId && cc.CardId == cardId);
            if (exists)
                return BadRequest("Carte déjà dans votre collection.");

            _context.CollectionCards.Add(new CollectionCard { UserId = userId, CardId = cardId });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Carte ajoutée à la collection." });
        }

        [HttpDelete("remove/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveCardFromCollection(int cardId)
        {
            int userId = User.GetUserId();

            var collectionCard = await _context.CollectionCards
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CardId == cardId);

            if (collectionCard == null)
                return NotFound("Carte non trouvée dans votre collection.");

            _context.CollectionCards.Remove(collectionCard);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Carte retirée de la collection." });
        }

        [HttpGet("filtered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CardInSetDto>>> GetFilteredCards(
            string? color,
            string? rarity,
            string? type,
            bool? owned // true = possédées, false = non possédées, null = toutes
        )
        {
            int userId = User.GetUserId();

            var query = _context.Cards
                .Include(c => c.Color)
                .Include(c => c.Rarity)
                .Include(c => c.CardType)
                .Include(c => c.SpecialRarity)
                .Include(c => c.CollectionCards)
                .AsQueryable();

            if (!string.IsNullOrEmpty(color))
                query = query.Where(c => c.Color != null && c.Color.Name == color);

            if (!string.IsNullOrEmpty(rarity))
                query = query.Where(c => c.Rarity != null && c.Rarity.Name == rarity);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(c => c.CardType != null && c.CardType.Name == type);

            if (owned != null)
            {
                if (owned.Value)
                    query = query.Where(c => c.CollectionCards.Any(cc => cc.UserId == userId));
                else
                    query = query.Where(c => c.CollectionCards.All(cc => cc.UserId != userId));
            }

            var cards = await query.ToListAsync();
            return Ok(cards.Select(CardMappers.ToDto));
        }

        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCollectionStats()
        {
            int userId = User.GetUserId();

            var total = await _context.CollectionCards.CountAsync(cc => cc.UserId == userId);

            var parColor = await _context.CollectionCards
                .Where(cc => cc.UserId == userId)
                .GroupBy(cc => cc.Card.Color!.Name)
                .Select(g => new StatDto { Name = g.Key!, Count = g.Count() })
                .ToListAsync();

            var parRarity = await _context.CollectionCards
                .Where(cc => cc.UserId == userId)
                .GroupBy(cc => cc.Card.Rarity!.Name)
                .Select(g => new StatDto { Name = g.Key!, Count = g.Count() })
                .ToListAsync();

            return Ok(new
            {
                total,
                parColor,
                parRarity
            });
        }

        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExportCollection()
        {
            int userId = User.GetUserId();

            var collectionCards = await _context.CollectionCards
                .Where(cc => cc.UserId == userId)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.Color)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.Rarity)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.SpecialRarity)
                .Include(cc => cc.Card)
                    .ThenInclude(c => c.CardSet)
                .ToListAsync();

            var result = collectionCards.Select(CollectionCardMappers.ToDto);
            return Ok(result);
        }

        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportCollection([FromBody] List<int> cardIds)
        {
            int userId = User.GetUserId();

            var existingCardIds = await _context.Cards
                .Where(c => cardIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            var alreadyOwned = await _context.CollectionCards
                .Where(cc => cc.UserId == userId && cardIds.Contains(cc.CardId))
                .Select(cc => cc.CardId)
                .ToListAsync();

            var newCardIds = existingCardIds.Except(alreadyOwned).ToList();

            var newEntries = newCardIds.Select(id => new CollectionCard
            {
                UserId = userId,
                CardId = id
            });

            _context.CollectionCards.AddRange(newEntries);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                imported = newCardIds.Count,
                alreadyOwned = alreadyOwned.Count
            });
        }
        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<CardInSetDto>> GetByCardCode(string code)
        {
            var card = await _context.Cards
                .Include(c => c.Color)
                .Include(c => c.Rarity)
                .Include(c => c.SpecialRarity)
                .Include(c => c.CardType)
                .FirstOrDefaultAsync(c => c.CardCode != null && c.CardCode.ToLower() == code.ToLower());

            if (card == null)
                return NotFound($"Carte avec code '{code}' introuvable.");

            return Ok(CardMappers.ToDto(card));
        }

        [HttpGet("check-ownership/by-code/{code}")]
        public async Task<IActionResult> CheckOwnershipByCardCode(string code)
        {
            int userId = User.GetUserId();

            var card = await _context.Cards
                .FirstOrDefaultAsync(c => c.CardCode != null && c.CardCode.ToLower() == code.ToLower());

            if (card == null)
                return NotFound($"Carte avec code '{code}' introuvable.");

            bool owns = await _context.CollectionCards.AnyAsync(cc => cc.UserId == userId && cc.CardId == card.Id);

            return Ok(new
            {
                card = CardMappers.ToDto(card),
                alreadyOwned = owns
            });
        }
    }
}
