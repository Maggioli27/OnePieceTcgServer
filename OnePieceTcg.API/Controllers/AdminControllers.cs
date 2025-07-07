using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePieceTcg.API.DTOs;
using OnePieceTcg.DAL.Data;
using OnePieceTcg.Domain.Enum;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // -------------------------
    // 📦 GESTION DES EXTENSIONS
    // -------------------------

    [HttpPost("extension")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExtension([FromBody] CardSetDto dto)
    {
        var cardSet = new CardSet
        {
            Name = dto.Name,
            Code = dto.Code,
            ReleaseDate = dto.ReleaseDate
        };

        _context.CardSets.Add(cardSet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllExtensions), new { id = cardSet.Id }, cardSet);
    }

    [HttpPut("extension/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExtension(int id, [FromBody] CardSetDto dto)
    {
        var extension = await _context.CardSets.FindAsync(id);
        if (extension == null) return NotFound("Extension introuvable.");

        extension.Name = dto.Name;
        extension.Code = dto.Code;
        extension.ReleaseDate = dto.ReleaseDate;

        await _context.SaveChangesAsync();
        return Ok(extension);
    }

    [HttpDelete("extension/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteExtension(int id)
    {
        var extension = await _context.CardSets.FindAsync(id);
        if (extension == null) return NotFound("Extension introuvable.");

        _context.CardSets.Remove(extension);
        await _context.SaveChangesAsync();
        return Ok("Extension supprimée.");
    }

    [HttpGet("extensions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllExtensions()
    {
        var extensions = await _context.CardSets.ToListAsync();
        return Ok(extensions);
    }

    // --------------------
    // 🃏 GESTION DES CARTES
    // --------------------

    [HttpPost(Name = "CreateCard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        return CreatedAtAction(nameof(CreateCard), new { setId = card.CardSetId }, new
        {
            message = "Carte créée avec succès",
            id = card.Id
        });
    }

    [HttpPut("{id}", Name = "UpdateCard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCard(int id, [FromBody] CardCreateDto dto)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card == null)
            return NotFound(new { message = $"Aucune carte trouvée avec l'ID {id}." });


        var cardSet = await _context.CardSets.SingleOrDefaultAsync(cs => cs.Name == dto.CardSetName);
        if (cardSet == null)
            return BadRequest(new { message = $"Extension '{dto.CardSetName}' introuvable." });

        var color = string.IsNullOrEmpty(dto.ColorName)
            ? null
            : await _context.Colors.SingleOrDefaultAsync(c => c.Name == dto.ColorName);

        var rarity = string.IsNullOrEmpty(dto.RarityName)
            ? null
            : await _context.Rarities.SingleOrDefaultAsync(r => r.Name == dto.RarityName);

        var specialRarity = string.IsNullOrEmpty(dto.SpecialRarityName)
            ? null
            : await _context.SpecialRarities.SingleOrDefaultAsync(sr => sr.Name == dto.SpecialRarityName);


        card.Name = dto.Name;
        card.Series = dto.Series;
        card.ImageUrl = dto.ImageUrl;
        card.CardSetId = cardSet.Id;
        card.ColorId = color?.Id;
        card.RarityId = rarity?.Id;
        card.SpecialRarityId = specialRarity?.Id;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Carte mise à jour avec succès", id = card.Id });
    }

    [HttpDelete("{id}", Name = "DeleteCard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card == null)
            return NotFound($"Aucune carte trouvée avec l'ID {id}.");

        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Carte supprimée avec succès", id = card.Id, name = card.Name });
    }

    [HttpGet("cards")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCards()
    {
        var cards = await _context.Cards
            .Include(c => c.CardSet)
            .Include(c => c.Color)
            .Include(c => c.Rarity)
            .Include(c => c.SpecialRarity)
            .ToListAsync();
        return Ok(cards);
    }

    [HttpPost("cards/import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportCards([FromBody] JsonElement jsonCards)
    {
        try
        {
            var cards = JsonSerializer.Deserialize<List<CardCreateDto>>(jsonCards.GetRawText());
            if (cards == null || cards.Count == 0)
                return BadRequest("Aucune carte à importer.");

            foreach (var dto in cards)
            {
                // Recherche des entités liées
                var cardSet = await _context.CardSets.SingleOrDefaultAsync(cs => cs.Name == dto.CardSetName);
                if (cardSet == null)
                    continue; // ou return BadRequest($"Extension '{dto.CardSetName}' introuvable.");

                var color = string.IsNullOrEmpty(dto.ColorName)
                    ? null
                    : await _context.Colors.SingleOrDefaultAsync(c => c.Name == dto.ColorName);

                var rarity = string.IsNullOrEmpty(dto.RarityName)
                    ? null
                    : await _context.Rarities.SingleOrDefaultAsync(r => r.Name == dto.RarityName);

                var specialRarity = string.IsNullOrEmpty(dto.SpecialRarityName)
                    ? null
                    : await _context.SpecialRarities.SingleOrDefaultAsync(sr => sr.Name == dto.SpecialRarityName);

                // Création de la carte
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
            }

            await _context.SaveChangesAsync();
            return Ok($"{cards.Count} cartes importées.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erreur d'import : {ex.Message}");
        }
    }


    // ---------------------
    // 🔐 GESTION UTILISATEUR
    // ---------------------

    [HttpPut("promote/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PromoteToAdmin(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("Utilisateur introuvable.");

        user.Role = UserRole.Admin;
        await _context.SaveChangesAsync();
        return Ok("Utilisateur promu Admin.");
    }

    [HttpDelete("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("Utilisateur introuvable.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok("Utilisateur supprimé.");
    }

    [HttpGet("user-stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserStats()
    {
        var totalUsers = await _context.Users.CountAsync();
        var adminCount = await _context.Users.CountAsync(u => u.Role == UserRole.Admin);
        var withCollection = await _context.CollectionCards.Select(cc => cc.UserId).Distinct().CountAsync();

        return Ok(new
        {
            totalUsers,
            adminCount,
            usersWithCollection = withCollection
        });
    }
}

