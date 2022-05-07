namespace CommonComponents.Models
{
    public class Hotel
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationCity { get; set; }
        public float Rating { get; set; }
        public string Food { get; set; }
        public short Stars { get; set; }
        public bool IsOnlyPremiumAvailable { get; set; }
        public int LowestPrice { get; set; }
    }
}