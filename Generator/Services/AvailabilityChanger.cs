using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Quartz;
using CommonComponents;

namespace Generator.Services
{

    public class AvailabilityChanger : IJob
    {

        private IHotelRepository _hotelRepository;
        private ITransportRepository _transportRepository;
        private Random _random;
        private IPublishEndpoint _publishEndpoint;

        public AvailabilityChanger(IHotelRepository hotelRepository, ITransportRepository transportRepository, IPublishEndpoint publishEndpoint)
        {
            _hotelRepository = hotelRepository;
            _transportRepository = transportRepository;
            _publishEndpoint = publishEndpoint;
            _random = new Random();
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            Hotel hotel = _hotelRepository.GetRandomHotel();
            int daysToChange = new Random().Next(5);
            DateTime startDate = new DateTime(2022, 6, 1);
            DateTime endDate = new DateTime(2022, 6, 30 - daysToChange);
            TimeSpan timeSpan = endDate - startDate;
            TimeSpan newSpan = new TimeSpan(0, _random.Next(0, (int) timeSpan.TotalMinutes), 0);
            DateTime newStartDate = startDate + newSpan;
            DateTime newEndDate = newStartDate.AddDays(daysToChange);
            
            List<Hotelroomavailability> roomAvailabilities = GetRandomRoomAvailabilitiesForHotel(hotel, newStartDate, daysToChange);
            int freeRooms = GetNumberOfFreeRooms(roomAvailabilities, hotel);
            Console.WriteLine($"Hotel {hotel.Id} rooms {freeRooms}" );
            List<short> identifiers = new List<short>();
            roomAvailabilities.ForEach( av => identifiers.Add(av.Id));
            
            if (freeRooms > 3)
            {
                _publishEndpoint.Publish(new ChangeHotelAvailabilityQuery
                {
                    IdentifierList = identifiers,
                    startDate = newStartDate,
                    endDate = newEndDate,
                    ChangeQuantity = (short) -_random.Next(3)
                });
            }
            else
            {
                _publishEndpoint.Publish(new ChangeHotelAvailabilityQuery
                {
                    IdentifierList = identifiers,
                    startDate = newStartDate,
                    endDate = newEndDate,
                    ChangeQuantity = (short) _random.Next(3)
                });         
            }
            GetRandomTransport();
            return Task.CompletedTask;
        }

        private List<Hotelroomavailability> GetRandomRoomAvailabilitiesForHotel(Hotel hotel, DateTime newDate, int daysToChange)
        {
            Hotelroom hotelRoom = GetRandomRoom(hotel);
            
            List<Hotelroomavailability> hotelroomavailabilities = new List<Hotelroomavailability>();
            for (DateOnly date = DateOnly.FromDateTime(newDate);
                date < DateOnly.FromDateTime(newDate.AddDays(daysToChange));
                date = date.AddDays(1))
            {
                Hotelroomavailability avail = hotelRoom.Hotelroomavailabilities.FirstOrDefault(av => av.Date == date);
                if (avail != null)
                {
                    hotelroomavailabilities.Add(avail);
                }
            }

            return hotelroomavailabilities;
        }
        private int GetNumberOfFreeRooms(List<Hotelroomavailability> hotelroomavailabilities, Hotel hotel) 
        {
            int freeRooms = Int32.MaxValue;
            foreach (Hotelroomavailability av in hotelroomavailabilities)
            {
                int reservations = _hotelRepository.GetEventsForDate(av.Date, av.Hotelroom.RoomtypeId, hotel.Id).Count;
                if (av.Quantity - reservations < freeRooms)
                {
                    freeRooms = av.Quantity - reservations;
                }
            }

            return freeRooms;
        }

        private Hotelroom GetRandomRoom(Hotel hotel)
        {
            List<Hotelroom> roomsList = hotel.Hotelrooms.ToList();
            return roomsList[_random.Next(roomsList.Count)];
        }

        public void GetRandomTransport()
        {
            Transport t = _transportRepository.GetRandomTransport();
            Console.WriteLine($"{t.Id} name: {t.Transporttype}");
        }

    }
}