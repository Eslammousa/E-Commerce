namespace E_Commerce.Core.Exceptions
{
    public class DuplicateEntityException : Exception
    {

        public DuplicateEntityException()
           : base()
        {
        }
        public DuplicateEntityException(string? message)
            : base(message)
        {
        }

        public DuplicateEntityException(string? message, Exception? innerException)
       : base(message, innerException)

        {
        }
    }
}
