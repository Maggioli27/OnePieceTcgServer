namespace OnePieceTcg.BLL.CustomExceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base("Invalid login.")
        {
        }

        public InvalidLoginException(string message) : base(message)
        {
        }
    }
}