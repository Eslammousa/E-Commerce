using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException()
           : base()
        {
        }
        public InvalidIdException(string? message)
            : base(message)
        {
        }
        public InvalidIdException(string? message, Exception? innerException)
       : base(message, innerException)
        {
        }
    }
}
