using System.Threading.Tasks;

namespace ApiGateway.Hubs.Clients
{
    public interface INotificationClient
    {
        Task ReceiveMessage(PurchaseNotification message);

        Task SendMessage(PurchaseNotification message);
        Task SendPopularCountryMessage(PopularCountryNotification message);
        Task SendPopularTripConfigurationMessage(PopularTripConfigurationNotification message);
        Task SendHotelStateChangeMessage(HotelStateChangeNotification message);
        Task SendTransportStateChangeMessage(TransportStateChangeNotification message);
        Task SendTripOffer(UpdatedTrip message);
    }
}