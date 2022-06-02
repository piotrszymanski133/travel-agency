using System.Collections.Generic;
using CommonComponents;
namespace ApiGateway.Controllers
{
    public class LastChangesList
    {
        public List<ChangeHotelAvailabilityQuery> _changeHotel { get; set; }
        public List<ChangeTransportPlacesQuery> _changeTransport { get; set; }

        public LastChangesList(List<ChangeHotelAvailabilityQuery> changeHotel, List<ChangeTransportPlacesQuery> changeTransport)
        {
            _changeHotel = changeHotel;
            _changeTransport = changeTransport;
        }
    }
}