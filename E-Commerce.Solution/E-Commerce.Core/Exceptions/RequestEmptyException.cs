namespace E_Commerce.Core.Exceptions
{
    public class RequestEmptyException : Exception
    {
        public RequestEmptyException()
           : base()
        {
        }
        public RequestEmptyException(string? message)
            : base(message)
        {
        }
        public RequestEmptyException(string? message, Exception? innerException)
       : base(message, innerException)
        {
        }
    }
}
