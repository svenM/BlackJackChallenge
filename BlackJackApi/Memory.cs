using BlackJackApi.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi
{
    public class Memory
    {
        private IMemoryCache _cache;
        private const string GameCacheKey = "GAMEZ";
        public Memory(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public List<LiveBlackjackGame> GetGames()
        {
            if(!_cache.TryGetValue(GameCacheKey, out object val))
            {
                return new List<LiveBlackjackGame>();
            }
            else
            {
                return val as List<LiveBlackjackGame>;
            }
        }

        public void AddGame(LiveBlackjackGame game)
        {
            var games = GetGames();
            games.Add(game);
            SetGames(games);
        }

        public void SetGames(List<LiveBlackjackGame> games)
        {
            _cache.Set(GameCacheKey, games, new MemoryCacheEntryOptions { SlidingExpiration = new TimeSpan(1, 0, 0 , 0) });
        }
    }
}
