using BlackJackApi;
using BlackJackApi.Domain;
using IO.Swagger.Api;
using IO.Swagger.Client;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Linq;

namespace BlackJackApiConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "https://localhost:44398";
            var lobbyClient = new LobbyApi(url);

            var games = lobbyClient.ApiLobbyListGet();

            var gameId = lobbyClient.ApiLobbyNewgameNameMinbetMaxbetPost("HelloTomx", 10, 100).Replace("\"","");

            var gameClient = new GameApi(url);

            var player = gameClient.GameIdJoinPut(gameId, "John Snow", 1);


            gameClient.GameIdPlayerPlayerIdBetAmountPost(gameId, player.Id, 10);

            gameClient.GameIdPlayerPlayerIdStandPost(gameId, player.Id);
            var result = gameClient.GameIdDetailsGet(gameId);
            return;



        }

        private static void DoubleDown(int wager, BlackjackGamePlayer player)
        {
            if (player.Account.Balance >= wager)
            {
                player.DoubleDown(wager);
            }
            else
            {
                player.DoubleDown(player.Account.Balance);
            }
        }

        private static void ShowHand(BlackjackGamePlayer player)
        {
          /*  Console.WriteLine($"Score : {player.Hand.ScoreHighLow.Item1} - {player.Hand.ScoreHighLow.Item2}");
            foreach (var card in player.Hand.Cards)
            {
                Console.WriteLine($"card {card.Suit} {card.Rank}");
            }*/
        }
    }
}
