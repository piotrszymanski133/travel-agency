using System;
using System.Collections.Generic;
using System.Linq;
using CommonComponents.Exceptions;
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
        public HotelWithDescription getHotel(short hotelId);
        void confirmOrder(Guid messageReservationId);
        List<string> GetAllDestinations();
        List<UserTripHotel> GetUserOrders(string messageUsername);
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
                return offer;
            }
            throw new SuitableRoomConfigurationNotFoundException(
                    $"Could not found free room for {neededCapacity} people in hotel {selectedHotel.Id}");
        }

        public bool tryToReserveHotel(ReserveTripOfferParameters parameters, Guid tripReservationId)
        {
            try
            {
                int peopleNumber = parameters.Adults + parameters.ChildrenUnder3 + parameters.ChildrenUnder10 +
                                   parameters.ChildrenUnder18;
                Hotel hotel = _hotelRepository.GetHotelWithDescription(parameters.HotelId);
                HotelStateOnDay hotelStateOnDay = _hotelRepository.findFreeRoomsForReservationTime(hotel,
                    parameters.StartDate, parameters.EndDate);
                HotelRoom roomToReserve =
                    hotelStateOnDay.FreeRooms.Find(
                        room => room.RoomtypeId == parameters.RoomTypeId && room.Quantity > 0);
                if (roomToReserve == null || peopleNumber != roomToReserve.CapacityPeople)
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

        public HotelWithDescription getHotel(short hotelId)
        {
            return _hotelRepository.GetHotelWithDescription(hotelId);
        }

        public void confirmOrder(Guid messageReservationId)
        {
            _hotelRepository.confirmOrder(messageReservationId);
        }

        public List<string> GetAllDestinations()
        {
            List<string> destinations = _hotelRepository.GetAllHotels()
                .GroupBy(h => h.Destination.Country)
                .Select(x => x.FirstOrDefault())
                .Select(x => x.Destination.Country)
                .ToList();
            destinations.Add("dowolnie");
            return destinations;
        }

        public List<UserTripHotel> GetUserOrders(string messageUsername)
        {
            List<Event> events = _hotelRepository.GetUserOrders(messageUsername);
            List<UserTripHotel> orders = new List<UserTripHotel>();
            events.ForEach(e =>
            {
                orders.Add(new UserTripHotel()
                {
                    ReservationId = e.TripReservationId,
                    HotelId = e.HotelId,
                    HotelName = e.Hotel.Name,
                    City = e.Hotel.Destination.City,
                    Country = e.Hotel.Destination.Country,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    FoodType = e.Hotel.Food
                });
            });
            return orders;
        }

        public void rollbackReservation(Guid messageTripReservationId)
        {
            _hotelRepository.rollbackReservation(messageTripReservationId);
        }
    }
}