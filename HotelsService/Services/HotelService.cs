using System;
using System.Collections.Generic;
using System.Linq;
using CommonComponents.Models;
using HotelsService.Models;
using HotelsService.Repositories;
using MassTransit.Util;
using Hotel = HotelsService.Models.Hotel;

namespace HotelsService.Services
{
    public interface IHotelService
    {
        HotelOffer createHotelOffer(TripOfferQueryParameters tripOfferQueryParameters,
            HotelWithDescription selectedHotel);

        bool tryToReserveHotel(ReserveTripOfferParameters parameters, Guid reservationId);
        void rollbackReservation(Guid messageTripReservationId);
        public HotelWithDescription getHotel(string hotelId);
        void confirmOrder(Guid messageReservationId);
        List<string> GetAllDestinations();
    }

    public class HotelService : IHotelService
    {
        private IHotelRepository _hotelRepository;
        private IHotelService _hotelServiceImplementation;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public HotelOffer createHotelOffer(TripOfferQueryParameters tripOfferQueryParameters,
            HotelWithDescription selectedHotel)
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
                offer.RoomsConfigurations = roomConfigurations;
            }

            return offer;
        }

        public bool tryToReserveHotel(ReserveTripOfferParameters parameters, Guid tripReservationId)
        {
            try
            {
                Hotel hotel = _hotelRepository.GetHotelWithDescription(parameters.HotelId);
                HotelStateOnDay hotelStateOnDay = _hotelRepository.findFreeRoomsForReservationTime(hotel,
                    parameters.StartDate, parameters.EndDate);
                HotelRoom roomToReserve =
                    hotelStateOnDay.FreeRooms.Find(
                        room => room.RoomtypeId == parameters.RoomTypeId && room.Quantity > 0);
                if (roomToReserve == null)
                {
                    return false;
                }

                _hotelRepository.CreateReservationEvent(hotel, tripReservationId, parameters.RoomTypeId,
                    parameters.StartDate, parameters.EndDate, parameters.Username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public HotelWithDescription getHotel(string hotelId)
        {
            return _hotelRepository.GetHotelWithDescription(hotelId);
        }

        public void confirmOrder(Guid messageReservationId)
        {
            _hotelRepository.confirmOrder(messageReservationId);
        }

        public List<string> GetAllDestinations()
        {
            return _hotelRepository.GetAllHotels()
                .GroupBy(h => h.Destination.Country)
                .Select(x => x.FirstOrDefault())
                .Select(x => x.Destination.Country)
                .ToList();
        }

        public void rollbackReservation(Guid messageTripReservationId)
        {
            _hotelRepository.rollbackReservation(messageTripReservationId);
        }
    }
}