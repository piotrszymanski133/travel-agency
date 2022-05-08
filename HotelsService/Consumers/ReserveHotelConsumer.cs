using System;
using System.Linq;
using System.Threading.Tasks;
using CommonComponents;
using HotelsService.Models;
using HotelsService.Repositories;
using HotelsService.Services;
using MassTransit;

namespace HotelsService.Consumers
{
    public class ReserveHotelConsumer : IConsumer<ReserveHotelQuery>
    {
        private IHotelService _hotelService;

        public ReserveHotelConsumer(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task Consume(ConsumeContext<ReserveHotelQuery> context)
        {
            bool success = _hotelService.tryToReserveHotel(context.Message.ReserveTripOfferParameters,
                context.Message.ReservationId);
            if (success)
            {
                Hotel hotel = _hotelService.getHotel(context.Message.ReserveTripOfferParameters.HotelId);
                string roomName = hotel.Hotelrooms
                    .Find(r => r.RoomtypeId == context.Message.ReserveTripOfferParameters.RoomTypeId)
                    .Roomtype
                    .Name;
                await context.Publish(new ReserveHotelSuccessResponse()
                {
                    Price = PriceCalculator.CalculateHotelRoomConfigPrice(hotel,
                        context.Message.ReserveTripOfferParameters),
                    HotelName = hotel.Name,
                    City = hotel.Destination.City,
                    Country = hotel.Destination.Country,
                    FoodType = hotel.Food,
                    ReservedRoomName = roomName,
                    ReservationId = context.Message.ReservationId,
                });
                Console.WriteLine($"Hotel reservation confirmed for id {context.Message.ReservationId}");

            }
            else
            {
                await context.Publish(new ReserveHotelFailureResponse()
                {
                    ReservationId = context.Message.ReservationId,
                });
                Console.WriteLine($"Hotel reservation failed for id {context.Message.ReservationId}");
            }
        }
    }
}