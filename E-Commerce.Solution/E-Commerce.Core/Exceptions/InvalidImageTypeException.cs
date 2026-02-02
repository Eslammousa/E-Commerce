namespace E_Commerce.Core.Exceptions
{
    public class InvalidImageTypeException : Exception
    {

        public InvalidImageTypeException()
           : base()
        {
        }
        public InvalidImageTypeException(string? message)
            : base(message)
        {
        }

        public InvalidImageTypeException(string? message, Exception? innerException)
       : base(message, innerException)

        {
        }
    }
}
