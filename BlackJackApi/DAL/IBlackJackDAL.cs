using BlackJackApi.Domain;
using System.Collections.Generic;

namespace BlackJackApi.DAL
{
    public interface IBlackJackDAL
    {
        void AddGame(LiveBlackjackGame game);
        List<LiveBlackjackGame> GetGames();
        LiveBlackjackGame GetGame(string id);
        void SetGames(List<LiveBlackjackGame> games);
        void RemoveGame(string id);
    }
}