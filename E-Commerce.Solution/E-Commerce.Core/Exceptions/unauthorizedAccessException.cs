namespace E_Commerce.Core.Exceptions
{
    public class unauthorizedAccessException : Exception
    {
        public unauthorizedAccessException()
    : base()
        {
        }
        public unauthorizedAccessException(string? message)
            : base(message)
        {
        }

        public unauthorizedAccessException(string? message, Exception? innerException)
       : base(message, innerException)

        {
        }

    }
}
