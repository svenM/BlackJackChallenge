using BlackJackApi.DAL;
using BlackJackApi.Domain;
using BlackJackApi.Domain.DTO;
using BlackJackApi.Domain.HubClients;
using Microsoft.AspNetCore.SignalR;

namespace BlackJackApi.Hubs
{
    public class LobbyHub : Hub<ILobbyHubClient>
    {
        private readonly IBlackJackDAL _blackJackDAL;

        public LobbyHub(IBlackJackDAL blackJackDAL)
        {
            _blackJackDAL = blackJackDAL;
        }
        public void GetGameList()
        {
            var games = _blackJackDAL.GetGames();
            Clients.All.JustSendHello("World");
            Clients.Caller.GameListSent(games);
        }

        public void CreateGame(string gameName, int minBet, int maxBet)
        {
            if (minBet <= 0)
            {
                SendError("Min bet < 0");
                return;
            }
            if (maxBet < minBet)
            {
                SendError("Max bet < min bet");
                return;
            }
            if (maxBet > 1000)
            {
                SendError("Max bet > 1000");
                return;
            }

            var game = new LiveBlackjackGame(gameName, minBet, maxBet, 30, 10);
            _blackJackDAL.AddGame(game);
            Clients.Caller.GameCreated(game.Id);
        }

        private void SendError(string msg)
        {
            Clients.Caller.LobbyError(msg);
        }

        public void DeleteGame(string gameId)
        {
            _blackJackDAL.RemoveGame(gameId);
            Clients.Caller.GameDeleted(gameId);
        }

    }
}