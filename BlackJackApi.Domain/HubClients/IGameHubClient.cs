using BlackJackApi.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJackApi.Domain.HubClients
{
    public interface IGameHubClient
    {
        Task GameJoined(string gameId, PlayerAccount account);
        Task HintReceived(string action);
        Task GameCreated(string gameId);
        Task PlayerHasBet(string gameId, string playerId, int amount);
        Task PlayerStanding(string gameId, string playerId);
        Task GameDetailsReceived(LiveBlackjackGame game);
        Task CardsDealt(LiveBlackjackGame game);
        Task RoundEnded(string gameId);
        Task PlayerRemoved(string gameId, string playerId);

        Task PlayerActionReceived(string gameId, string playerId, string action);

        Task GameError(string message);
    }
}
