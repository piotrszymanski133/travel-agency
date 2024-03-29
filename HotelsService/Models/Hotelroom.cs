﻿using System;
using System.Collections.Generic;
using CommonComponents.Models;
using HotelsService.Models;
using Hotel = HotelsService.Models.Hotel;

namespace HotelsService
{
    public partial class Hotelroom
    {
        public Hotelroom()
        {
            HotelRoomAvailabilities = new List<HotelRoomAvailability>();
        }
        public short Id { get; set; }
        public short HotelId { get; set; }
        public short RoomtypeId { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual Hotelroomtype Roomtype { get; set; } = null!;
        public virtual List<HotelRoomAvailability> HotelRoomAvailabilities { get; set; } = null!;
    }
}