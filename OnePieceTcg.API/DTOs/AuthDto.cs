using System.ComponentModel.DataAnnotations;

namespace OnePieceTcg.API.DTOs
{
    public class AuthRegisterForm
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 50 caractères.")]
        public required string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "L'adresse e-mail fournie n'est pas valide.")]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères.")]
        public required string Password { get; set; }


    }

    public class AuthLoginForm
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    /* OUTPUT DTO */
    public class TokenResponse
    {
        public required string Token { get; set; }
    }
}
