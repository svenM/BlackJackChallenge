using BlackJackApi.Domain.DTO;
using Microsoft.AspNetCore.SignalR.Client;
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
            var url = "https://localhost:44398";
            //Make proxy to hub based on hub name on server
            var connection = new HubConnectionBuilder()
                .WithUrl(url + "/lobbyHub")
                .WithAutomaticReconnect()
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
            connection.On<string>("LobbyError", error => {
                Console.WriteLine("error : " + error);
            });

            await connection.InvokeAsync<string>("GetGameList").ContinueWith(task => {
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