using System;

namespace CommonComponents.Exceptions
{
    public class SuitableRoomConfigurationNotFoundException : Exception
    {
        public SuitableRoomConfigurationNotFoundException()
        {
        }

        public SuitableRoomConfigurationNotFoundException(string message) : base(message)
        {
        }
        
        public SuitableRoomConfigurationNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}