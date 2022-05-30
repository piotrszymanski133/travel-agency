using System.Threading.Tasks;

namespace ApiGateway.Hubs.Clients
{
    public interface INotificationClient
    {
        Task ReceiveMessage(PurchaseNotificationMessage message);

        Task SendMessage(PurchaseNotificationMessage message);
        Task SendPopularCountryMessage(PopularCountryNotificationMessage message);
    }
}