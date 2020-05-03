using BlackJackApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BlackJackApi.DAL
{
    public class MemoryDAL : IBlackJackDAL
    {
        private readonly Memory _memory;
        public MemoryDAL(IMemoryCache cache)
        {
            _memory = new Memory(cache);
        }
        public List<LiveBlackjackGame> GetGames()
        {
            return _memory.GetGames();
        }

        public void AddGame(LiveBlackjackGame game)
        {
            _memory.AddGame(game);
        }
        
        public LiveBlackjackGame GetGame(string id)
        {
            var games = _memory.GetGames();
            if(games.Any(g => g.Id == id))
            {
                return games.First(g => g.Id == id);
            }
            else
            {
                return null;
            }
        }

        public void RemoveGame(string id)
        {
            var games = _memory.GetGames();
            games = games.Where(g => g.Id != id).ToList();
            SetGames(games);
        }

        public void SetGames(List<LiveBlackjackGame> games)
        {
            _memory.SetGames(games);
        }

        public void SaveGame(LiveBlackjackGame game)
        {
            //todo
        }
    }
}
