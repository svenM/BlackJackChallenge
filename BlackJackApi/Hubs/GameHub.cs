using BlackJackApi.DAL;
using BlackJackApi.Domain;
using BlackJackApi.Domain.DTO;
using BlackJackApi.Domain.HubClients;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace BlackJackApi.Hubs
{
    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IBlackJackDAL _blackJackDAL;

        public GameHub(IBlackJackDAL blackJackDAL)
        {
            _blackJackDAL = blackJackDAL;
        }

        public void JoinGame(string gameId, string playerName, int seatNo)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }

            double balance = 1000;

            var account = new PlayerAccount(id: Guid.NewGuid().ToString().Replace("-", ""), startingBalance: balance);

            game.AddPlayer(account, playerName, seatNo);
            _blackJackDAL.SaveGame(game);
            Clients.Caller.GameJoined(gameId, account);
        }

        public void RequestHint(string gameId, string playerId)
        {
            var decider = new PlayerActionDecider();
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }

            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null || player.Hand == null || !player.Hand.Cards.Any())
            {
                Clients.Caller.GameError("Player not found or no cards");
                return;
            }

            Clients.Caller.HintReceived(decider.DecideAction(player.Hand, game.DealerHand.Cards.First()).ToString());
        }

        public void Bet(string gameId, string playerId, int amount)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                Clients.Caller.GameError("Player not found");
                return;
            };

            game.PlayerWagerRequest(player, amount);
            _blackJackDAL.SaveGame(game);
            Clients.Caller.PlayerHasBet(gameId, playerId, amount);
        }

        public void Stand(string gameId, string playerId)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                Clients.Caller.GameError("Player not found");
                return;
            };

            game.ForceCurrentActionToStand();
            _blackJackDAL.SaveGame(game);
            Clients.Caller.PlayerStanding(gameId, playerId);
        }

        public void GetGameDetail(string gameId)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            Clients.Caller.GameDetailsReceived(game);
        }

        public void Deal(string gameId)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            game.StartRound();
            _blackJackDAL.SaveGame(game);
            Clients.All.CardsDealt(game);
        }

        public void EndRound(string gameId)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            game.EndRound();
            _blackJackDAL.SaveGame(game);
            Clients.All.RoundEnded(gameId);
        }

        public void RemovePlayer(string gameId, string playerId)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                Clients.Caller.GameError("Player not found");
                return;
            };
            game.RemovePlayer(player);
            _blackJackDAL.SaveGame(game);
            Clients.Caller.PlayerRemoved(gameId, playerId);
        }

        public void PlayerAction(string gameId, string playerId, string action)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                Clients.Caller.GameError("Game not found");
                return;
            }
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                Clients.Caller.GameError("Player not found");
                return;
            };
            game.PlayerActionRequest(player, action);
            _blackJackDAL.SaveGame(game);
            Clients.Caller.PlayerActionReceived(gameId, playerId, action);
        }

        private LiveBlackjackGame GetGame(string gameId)
        {
            return _blackJackDAL.GetGame(gameId);
        }

        public GameHub
    }
}