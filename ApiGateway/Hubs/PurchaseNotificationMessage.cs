namespace ApiGateway.Hubs
{
    public class PurchaseNotificationMessage
    {
        public string Message { get; set; }
        public int HotelId { get; set; }
    }
}