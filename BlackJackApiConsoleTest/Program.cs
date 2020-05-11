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
        private static int Position1 = 1;
        private static int Position2 = 2;

        static void nMain(string[] args)
        {
            var url = "http://localhost/BlackJackApi/";
            lobbyClient = new LobbyApi(url);

            var games = lobbyClient.ApiLobbyListGet();

            var gameId = lobbyClient.ApiLobbyNewgameNameMinbetMaxbetPost("HelloTomx", 10, 100).Replace("\"", "");

            gameClient = new GameApi(url);
            Player1 = gameClient.GameIdJoinPut(gameId, "John Snow", Position1);
            Player2 = gameClient.GameIdJoinPut(gameId, "Stephen Fry", Position2);
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

            gameClient.GameIdPlayerPlayerIdStandPost(gameId, Player1.Id);
            gameClient.GameIdPlayerPlayerIdStandPost(gameId, Player2.Id);
            var result = gameClient.GameIdDetailsGet(gameId);

            var outCome1 = result.RoundInProgressSettlements.First(p => p.PlayerId == Player1.Id);
            Console.WriteLine(outCome1.WagerOutcome);
            var outCome2 = result.RoundInProgressSettlements.First(p => p.PlayerId == Player2.Id);
            Console.WriteLine(outCome2.WagerOutcome);

            gameClient.GameIdEndPost(gameId);

            //round 2
            gameClient.GameIdPlayerPlayerIdBetAmountPost(gameId, Player1.Id, bet);

            // Bet Player 2
            gameClient.GameIdPlayerPlayerIdBetAmountPost(gameId, Player2.Id, bet);
            gameClient.GameIdDealGet(gameId);
            gameClient.GameIdPlayerPlayerIdStandPost(gameId, Player1.Id);
            gameClient.GameIdPlayerPlayerIdStandPost(gameId, Player2.Id);
            result = gameClient.GameIdDetailsGet(gameId);

            outCome1 = result.RoundInProgressSettlements.First(p => p.PlayerId == Player1.Id);
            Console.WriteLine(outCome1.WagerOutcome);
            outCome2 = result.RoundInProgressSettlements.First(p => p.PlayerId == Player2.Id);
            Console.WriteLine(outCome2.WagerOutcome);
            gameClient.GameIdEndPost(gameId);


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
