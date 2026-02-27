namespace E_Commerce.Core.Exceptions
{
    public class InsufficientStockException : Exception
    {

        public InsufficientStockException()
           : base()
        {
        }
        public InsufficientStockException(string? message)
            : base(message)
        {
        }

        public InsufficientStockException(string? message, Exception? innerException)
       : base(message, innerException)

        {
        }
    }
}
