namespace OnePieceTcg.BLL.CustomExceptions
{
    public class UserNameAlreadyUsedException : Exception
    {
        public UserNameAlreadyUsedException() : base("Username already used.")
        {
        }

        public UserNameAlreadyUsedException(string message) : base(message)
        {
        }
    }
}