using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using HotelsService.Repositories;
using MassTransit;
using HotelsService.Models;
using Hotel = CommonComponents.Models.Hotel;

namespace HotelsService.Consumers
{
    public class GetHotelsConsumer : IConsumer<GetHotelsQuery>
    {
        private IHotelRepository _hotelRepository;

        public GetHotelsConsumer(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public async Task Consume(ConsumeContext<GetHotelsQuery> context)
        {
            List<HotelWithDescription> allHotels = _hotelRepository.GetAllHotels();
            _hotelRepository.CreateReservationEvent();
            List<Hotel> hotels = new List<Hotel>();
            foreach (HotelWithDescription hotel in allHotels)
            {
                List<HotelRoom> rooms = new List<HotelRoom>();
                foreach (Hotelroom room in hotel.Hotelrooms)
                {
                    rooms.Add(new HotelRoom
                    {
                        Quantity = room.Quantity,
                        Name = room.Roomtype.Name,
                        CapacityPeople = room.Roomtype.CapacityPeople
                    });
                }
                hotels.Add(new Hotel
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    DestinationCity = hotel.Destination.City,
                    DestinationCountry = hotel.Destination.Country,
                    Rating = hotel.Rating,
                    Food = hotel.Food,
                    Stars = hotel.Stars.GetValueOrDefault(),
                    Description = hotel.Description,
                    Rooms = rooms
                });
            }
            await context.RespondAsync(new GetHotelsRespond { Hotels = hotels});
        }
    }
}