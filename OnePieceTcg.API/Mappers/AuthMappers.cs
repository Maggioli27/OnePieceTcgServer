using OnePieceTcg.API.DTOs;
using OnePieceTcg.Domain.Models;

namespace OnePieceTcg.API.Mappers
{
    public static class AuthMappers
    {
        public static User ToUtilisateur(this AuthRegisterForm form)
        {
            return new User
            {
                UserName = form.Username,
                Email = form.Email,
                Password = form.Password,

            };
        }
    }
}
