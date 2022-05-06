namespace CommonComponents.Models
{
    public class Transport
    {
        public string Name { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationCity { get; set; }
        public string DepartueCity { get; set; }
        public string DepartueCountry { get; set; }
        public long Id { get; set; }
        public int Quantity { get; set; }
    }
}