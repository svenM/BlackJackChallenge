using BlackJackApi.Domain;
using BlackJackApi.Domain.DTO;
using BlackJackApi.Infrastructure;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BlackJackApi.DAL
{
    public class LiteDBDAL : IBlackJackDAL
    {
        private readonly LiteDatabase _context;

        public LiteDBDAL(LiteDbContext context)
        {
            _context = context.Context;
        }

        private ILiteCollection<LiveBlackjackGame> games => _context.GetCollection<LiveBlackjackGame>("games");

        public List<LiveBlackjackGame> GetGames()
        {
            return games.Query().ToList();
        }

        public void AddGame(LiveBlackjackGame game)
        {
            games.Insert(game);
        }
        
        public void SaveGame(LiveBlackjackGame game)
        {
            games.Update(game);
        }
        public LiveBlackjackGame GetGame(string id)
        {
            var query = games.Query().Where(g => g.Id == id).FirstOrDefault();
            if (query != null) 
            {
                foreach (var players in query.Players)
                {
                    players.Game = query;
                }
                return query;
            }
            else
            {
                return null;
            }
        }

        public void RemoveGame(string id)
        {
            games.Delete(id);
        }

    }
}
