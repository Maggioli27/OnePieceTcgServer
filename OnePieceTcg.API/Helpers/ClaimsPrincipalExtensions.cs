using System.Security.Claims;

namespace OnePieceTcg.API.Helpers
{
    public static class ClaimsPrincipalExtensions // récupérer l'ID de l’utilisateur connecté
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User ID non trouvé dans le token.");

            return int.Parse(userIdClaim.Value);
        }
    }
}
