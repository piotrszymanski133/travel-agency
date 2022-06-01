using System.Threading.Tasks;

namespace ApiGateway.Hubs.Clients
{
    public interface INotificationClient
    {
        Task ReceiveMessage(PurchaseNotification message);

        Task SendMessage(PurchaseNotification message);
        Task SendPopularCountryMessage(PopularCountryNotification message);
        Task SendPopularTripConfigurationMessage(PopularTripConfigurationNotification message);
    }
}