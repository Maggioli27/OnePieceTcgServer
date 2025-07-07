using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePieceTcg.API.DTOs;
using OnePieceTcg.API.Services;
using OnePieceTcg.DAL.Data;
using OnePieceTcg.Domain.Enum;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthService _authService;

        public AuthController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, AuthService authService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Users.AnyAsync(u => u.UserName == form.Username);
            if (existingUser)
                return BadRequest("Nom d'utilisateur déjà utilisé.");


            var user = new User
            {
                UserName = form.Username,
                Email = form.Email,
                Role = UserRole.User
            };

            user.Password = _passwordHasher.HashPassword(user, form.Password);


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Inscription réussie !");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] AuthLoginForm form)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == form.Username);
            if (user == null)
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, form.Password);
            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");

            var token = _authService.GenerateToken(user);
            return Ok(new
            {
                message = "Connexion réussie",
                Token = token
            });
        }
    }
}
