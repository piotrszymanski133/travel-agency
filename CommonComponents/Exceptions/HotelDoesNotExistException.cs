using System;

namespace CommonComponents.Exceptions
{
    public class HotelDoesNotExistException: Exception
    {
        public HotelDoesNotExistException()
        {
        }

        public HotelDoesNotExistException(string message) : base(message)
        {
        }
        
        public HotelDoesNotExistException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    
}