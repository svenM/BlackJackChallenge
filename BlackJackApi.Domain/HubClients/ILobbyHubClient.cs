using BlackJackApi.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJackApi.Domain.HubClients
{
    public interface ILobbyHubClient
    {
        Task GameListSent(List<LiveBlackjackGame> games);
        Task JustSendHello(string hello);
        Task GameCreated(string gameId);
        Task LobbyError(string message);
        Task GameDeleted(string gameId);
    }
}
