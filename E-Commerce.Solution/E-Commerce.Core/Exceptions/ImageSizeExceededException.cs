namespace E_Commerce.Core.Exceptions
{
    public class ImageSizeExceededException : Exception
    {

        public ImageSizeExceededException()
           : base()
        {
        }
        public ImageSizeExceededException(string? message)
            : base(message)
        {
        }

        public ImageSizeExceededException(string? message, Exception? innerException)
       : base(message, innerException)

        {
        }
    }
}
