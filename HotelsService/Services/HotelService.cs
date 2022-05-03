using System.Collections.Generic;
using CommonComponents.Models;
using HotelsService.Models;
using HotelsService.Repositories;

namespace HotelsService.Services
{
    public interface IHotelService
    {
        HotelOffer createHotelOffer(TripOfferQueryParameters tripOfferQueryParameters, HotelWithDescription selectedHotel);
    }
    public class HotelService : IHotelService
    {
        private IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public HotelOffer createHotelOffer(TripOfferQueryParameters tripOfferQueryParameters, HotelWithDescription selectedHotel)
        {
            HotelOffer offer = new HotelOffer
            {
                Id = selectedHotel.Id,
                Name = selectedHotel.Name,
                DestinationCountry = selectedHotel.Destination.Country,
                DestinationCity = selectedHotel.Destination.City,
                Rating = selectedHotel.Rating,
                Food = selectedHotel.Food,
                Stars = selectedHotel.Stars.GetValueOrDefault(),
                Description = selectedHotel.Description
            };
            
            int neededCapacity = tripOfferQueryParameters.Adults + tripOfferQueryParameters.ChildrenUnder3 +
                                 tripOfferQueryParameters.ChildrenUnder10 +
                                 tripOfferQueryParameters.ChildrenUnder18;
            
            HotelStateOnDay hotelStateOnDay = _hotelRepository.findFreeRoomsForReservationTime(selectedHotel,
                tripOfferQueryParameters.StartDate, tripOfferQueryParameters.EndDate);
            
            List<HotelRoom> roomConfigurations = hotelStateOnDay.FreeRooms.FindAll(room =>
                room.CapacityPeople == neededCapacity && room.Quantity > 0);
            if (roomConfigurations.Count != 0)
            {
                roomConfigurations.ForEach(room => room.Quantity = 1);
                offer.RoomsConfigurations = roomConfigurations;
            }

            return offer;
        }
    }
}