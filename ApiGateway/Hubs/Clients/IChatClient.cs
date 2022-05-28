using System.Threading.Tasks;

namespace ApiGateway.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);

        Task SendMessage(ChatMessage message);
    }
}