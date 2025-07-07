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


        [HttpGet("withCardCounts", Name = "GetCardSetsWithCounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CardSetDto>>> GetCardSetsWithCounts()
        {
            var sets = await _context.CardSets
                .Include(cs => cs.Cards)
                .OrderByDescending(cs => cs.ReleaseDate)
                .Select(cs => new CardSetDto
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Code = cs.Code,
                    ReleaseDate = cs.ReleaseDate,
                    CardCount = cs.Cards.Count
                })
                .ToListAsync();

            return Ok(sets);
        }
    }

}
