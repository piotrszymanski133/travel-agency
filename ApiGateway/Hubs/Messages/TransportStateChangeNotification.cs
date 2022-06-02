using System;

namespace ApiGateway.Hubs
{
    public class TransportStateChangeNotification
    {
        public long TransportId { get; set; }
        public long DestinationPlacesId { get; set; }
        public long SourcePlacesId { get; set; }
    }
}