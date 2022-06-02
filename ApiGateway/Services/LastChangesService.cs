using System.Collections.Generic;
using System.Linq;
using CommonComponents;
    
namespace ApiGateway.Services
{
    
    public interface ILastChangesService
    {
        public  List<ChangeTransportPlacesQuery> _changeTransport { get; set; }
        public  List<ChangeHotelAvailabilityQuery> _changeHotel { get; set; }
        void AddTransportChangeEvent(ChangeTransportPlacesQuery msg);
        void AddHotelChangeEvent(ChangeHotelAvailabilityQuery msg);
    }
    public class LastChangesService : ILastChangesService
    {
        public  List<ChangeTransportPlacesQuery> _changeTransport { get; set; }
        public  List<ChangeHotelAvailabilityQuery> _changeHotel { get; set; }

        
        public LastChangesService()
        {
            _changeTransport = new List<ChangeTransportPlacesQuery>();
            _changeHotel = new List<ChangeHotelAvailabilityQuery>();
        }

        public void AddTransportChangeEvent(ChangeTransportPlacesQuery msg)
        {
            if (_changeTransport.Count <= 10)
            {
                _changeTransport.Add(msg);
            }
            else
            {
                _changeTransport.Remove(_changeTransport.First());
                _changeTransport.Add(msg);
            }
        }

        public void AddHotelChangeEvent(ChangeHotelAvailabilityQuery msg)
        {
            if (_changeHotel.Count <= 10)
            {
                _changeHotel.Add(msg);
            }
            else
            {
                _changeHotel.Remove(_changeHotel.First());
                _changeHotel.Add(msg);
            }
        }
    }
    
    
    
}