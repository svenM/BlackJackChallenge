using BlackJackApi.Domain.DTO;
using System.Collections.Generic;

namespace BlackJackApi.DAL
{
    public interface IBlackJackDAL
    {
        void AddGame(LiveBlackjackGame game);
        List<LiveBlackjackGame> GetGames();
        LiveBlackjackGame GetGame(string id);
        void RemoveGame(string id);
        void SaveGame(LiveBlackjackGame game);
    }
}