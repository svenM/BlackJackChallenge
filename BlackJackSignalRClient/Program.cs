using BlackJackApi.Domain.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlackJackSignalRClient
{
    class Program
    {
        private static ConnectionState _conState = new ConnectionState();
        private static HubConnection _lobbyHub;
        private static HubConnection _gameHub;
        static async Task Main(string[] args)
        {

            //Set connection
            var url = "http://localhost/BlackJackApi";
            //Make proxy to hub based on hub name on server

            _lobbyHub = new HubConnectionBuilder()
                .WithUrl(url + "/lobbyHub")
                .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    // Log to the Console
                    logging.AddDebug();

                    // This will set ALL logging to Debug level
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();

            _gameHub = new HubConnectionBuilder()
                .WithUrl(url + "/gameHub")
                .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    // Log to the Console
                    logging.AddDebug();

                    // This will set ALL logging to Debug level
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();


            //Start connection
            _lobbyHub.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                    _conState.LobbyHubError = true;
                }
                else
                {
                    _conState.LobbyHubConnected = true;
                }

            }).Wait(); 
            
            _gameHub.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                    _conState.GameHubError = true;
                }
                else
                {
                    _conState.GameHubConnected = true; ;
                }

            }).Wait();

            while(!(_conState.Ready ||  _conState.Faulted))
            {
                Thread.Sleep(100);
            }
            if(_conState.Faulted)
            {
                Console.WriteLine("The end, press any key");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("So we managed to connect");

            SetupLobbyEvents(_lobbyHub);
            SetupGameHub(_gameHub);

            await _lobbyHub.InvokeAsync<string>("getGameList");
            await _lobbyHub.InvokeAsync<string>("CreateGame", "XXX",10,200);


            Console.Read();
            await _lobbyHub.StopAsync();
        }

        private static void SetupGameHub(HubConnection gameHub)
        {
            gameHub.On<string, PlayerAccount>("GameJoined", (string gameId, PlayerAccount account) =>
            {
                Console.WriteLine("You joined " + gameId + ", your balance is : " + account.Balance);
            });
        }

        private static void SetupLobbyEvents(HubConnection lobbyHub)
        {
            lobbyHub.On<string>("JustSendHello", hello =>
            {
                Console.WriteLine("Hello : " + hello);
            });
            lobbyHub.On<string>("LobbyError", error =>
            {
                Console.WriteLine("error : " + error);
            });
            lobbyHub.On<List<LiveBlackjackGame>>("GameListSent", list =>
            {
                Console.WriteLine("Games : ");
                foreach (var game in list)
                {
                    Console.WriteLine(game.Name);
                }
            });
            lobbyHub.On<string>("GameCreated", gameId =>
            {
                Console.WriteLine($"game {gameId} created");
                OnGameCreated(gameId);
            }); 
            lobbyHub.On<string>("GameDeleted", gameId =>
            {
                Console.WriteLine($"game {gameId} created");
            });
        }

        private static void OnGameCreated(string gameId)
        {
            _gameHub.InvokeAsync<string>("JoinGame", gameId, "Jeff GoldBlum",1);
        }
    }
}