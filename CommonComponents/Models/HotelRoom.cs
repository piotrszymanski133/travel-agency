namespace CommonComponents.Models
{
    public class HotelRoom
    {
        public short CapacityPeople { get; set; }
        public short Quantity { get; set; }
        public short RoomtypeId { get; set; }
        public string Name { get; set; }

        public HotelRoom shallowCopy()
        {
            return MemberwiseClone() as HotelRoom;
        }
    }
}