using BlackJackApi.Domain.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJackSignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            //Set connection
            var url = "http://localhost/BlackJackApi";
            //Make proxy to hub based on hub name on server
            
            var connection = new HubConnectionBuilder()
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
            //Start connection
            connection.StartAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();
            connection.On<List<LiveBlackjackGame>>("GameListSent", list => {
                Console.WriteLine("Games : ");
                foreach (var game in list)
                {
                    Console.WriteLine(game.Name);
                }
            });
            connection.On<string>("JustSendHello", hello => {
                Console.WriteLine("Hello : " + hello);
            });
            connection.On<string>("LobbyError", error => {
                Console.WriteLine("error : " + error);
            });

            await connection.InvokeAsync<string>("getGameList").ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error calling send: {0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine(task.Result);
                }
            });



            Console.Read();
            await connection.StopAsync();
        }
    }
}