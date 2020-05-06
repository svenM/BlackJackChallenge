using BlackJackApi.DAL;
using BlackJackApi.Domain;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly IBlackJackDAL _blackJackDAL;

        public LobbyHub(IBlackJackDAL blackJackDAL)
        {
            _blackJackDAL = blackJackDAL;
        }
        public void GetGameList(string groupName)
        {
            Clients.Caller.SendAsync("GameListSent", _blackJackDAL.GetGames());
        }

        public void CreateGame(string gameName, int minBet, int maxBet)
        {
            if (minBet <= 0) SendError("Min bet < 0");
            if (maxBet < minBet) SendError("Max bet < min bet");
            if (maxBet > 200) SendError("Max bet > 200");

            var game = new LiveBlackjackGame(gameName, minBet, maxBet, 30, 10);
            _blackJackDAL.AddGame(game);
            Clients.Caller.SendAsync("GameCreated", game.Id);
        }

        private void SendError(string msg)
        {
            Clients.Caller.SendAsync("Error", msg);
        }
        public void GameDetailRequest(string gameId)
        {
            var game = _blackJackDAL.GetGame(gameId);
            if (game != null)
            {
                Clients.Caller.SendAsync("GameDetail", game);
            }
            else
            {
                SendError($"Game {gameId} not found");
            }
        }

        public void DeleteGame(string gameId)
        {
            _blackJackDAL.RemoveGame(gameId);
            Clients.Caller.SendAsync($"Game {gameId} removed");
        }

    }
}