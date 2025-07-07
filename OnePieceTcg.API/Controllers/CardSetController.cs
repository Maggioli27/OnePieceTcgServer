using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePieceTcg.API.DTOs;
using OnePieceTcg.API.Mappers;
using OnePieceTcg.DAL.Data;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Controllers //
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

        [HttpGet(Name = "GetAllExtensions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CardSetDto>>> GetAllCardSets()
        {
            var sets = await _context.CardSets.ToListAsync();
            var setDtos = sets.Select(CardSetMappers.ToDto).ToList();
            return Ok(setDtos);
        }



        [HttpPost(Name = "CreateExtension")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCardSet([FromBody] CardSetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cardSet = new CardSet
            {
                Name = dto.Name,
                Code = dto.Code
            };

            _context.CardSets.Add(cardSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllCardSets), new { id = cardSet.Id }, new
            {
                message = "Extension ajoutée avec succès",
                id = cardSet.Id,
                name = cardSet.Name
            });
        }
        [HttpDelete("{id}", Name = "DeleteExtension")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCardSet(int id)
        {
            var cardSet = await _context.CardSets.FindAsync(id);
            if (cardSet == null)
                return NotFound($"Aucune extension trouvée avec l'ID {id}.");

            _context.CardSets.Remove(cardSet);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Extension supprimée avec succès" });
        }
    }

}
