using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Linq;
using System.Threading;

namespace BlackJackApiConsoleTest
{
    class Program
    {
        private static PlayerAccount Player1;
        private static PlayerAccount Player2;
        private static LobbyApi lobbyClient;
        private static GameApi gameClient;

        static void Main(string[] args)
        {
            var url = "https://localhost:44398";
            lobbyClient = new LobbyApi(url);

            var games = lobbyClient.ApiLobbyListGet();

            var gameId = lobbyClient.ApiLobbyNewgameNameMinbetMaxbetPost("HelloTomx", 10, 100).Replace("\"", "");

            gameClient = new GameApi(url);
            var position = 1;
            var position2 = 2;
            Player1 = gameClient.GameIdJoinPut(gameId, "John Snow", position);
            Player2 = gameClient.GameIdJoinPut(gameId, "Stephen Fry", position2);
            PlayRound(gameId);
            return;
        }

        public static void PlayRound(string gameId)
        {
            var bet = 10;
            // Bet Player 1
            gameClient.GameIdPlayerPlayerIdBetAmountPost(gameId, Player1.Id, bet);

            // Bet Player 2
            gameClient.GameIdPlayerPlayerIdBetAmountPost(gameId, Player2.Id, bet);

            gameClient.GameIdDealGet(gameId);

            var result = gameClient.GameIdDetailsGet(gameId);

        }

        private static double? PlayRound(string gameId, GameApi gameClient, int position, PlayerAccount player, int bet)
        {
            return 0;
            //gameClient.GameIdPlayerPlayerIdBetAmountPost(gameId, player.Id, bet);

            //gameClient.GameIdPlayerPlayerIdStandPost(gameId, player.Id);
            //result = gameClient.GameIdDetailsGet(gameId);

            //outcome = result.RoundInProgressSettlements.FirstOrDefault(s => s.PlayerPosition == position);
            //if (outcome == null)
            //{
            //    Console.WriteLine("oops");
            //}
            //else
            //{
            //    Console.WriteLine("outcome : " + outcome.WagerOutcome);
            //}
            //return result.Players.First(p => p.Id == player.Id).Account.Balance;
        }

    }
}
