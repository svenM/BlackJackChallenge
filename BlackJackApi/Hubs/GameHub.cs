using BlackJackApi.DAL;
using BlackJackApi.Domain;
using Microsoft.AspNetCore.SignalR;

namespace BlackJackApi.Hubs
{
    public class GameHub : Hub
    {
        private readonly IBlackJackDAL _blackJackDAL;

        public GameHub(IBlackJackDAL blackJackDAL)
        {
            _blackJackDAL = blackJackDAL;
        }

    }
}