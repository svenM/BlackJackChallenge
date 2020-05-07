using BlackJackApi.Domain;
using LiteDB;
using System;
using System.Collections.Generic;

namespace BlackJackApi.Domain.DTO
{
    public class BlackjackGamePlayer
    {
        public BlackjackGamePlayer()
        {

        }
        public string Id { get; private set; }

        [BsonIgnore]
        public BlackjackGame Game { get; set; }

        public int Position { get; private set; }
        public PlayerAccount Account { get; private set; }

        public string Alias { get; private set; }

        public bool IsLive { get { return Game?.GetPlayerIsLive(this) ?? false; } }
        public bool HasAction { get { return Game?.GetPlayerHasAction(this) ?? false; } }
        public double Wager { get { return Game?.GetPlayerWager(this) ?? 0; } }
        public BlackjackHand Hand { get { return Game?.GetPlayerHand(this) ; } }

        public BlackjackGamePlayer(PlayerAccount account, BlackjackGame game, string alias, int position)
        {
            if (account == null)
                throw new ArgumentNullException("account", "Account is null");

            if (game == null)
                throw new ArgumentNullException("game", "Game is null");

            Game = game;
            Alias = string.IsNullOrEmpty(alias) ? "ANON" : alias;
            Account = account;
            Position = position;
            Id = Account.Id;
        }

        public void SetWager(double amount)
        {
            Game.PlaceWager(this, amount);
        }

        public void Hit()
        {
            Game.RequestToHit(this);
        }

        public void Stand()
        {
            Game.RequestToStand(this);
        }

        public void DoubleDown(double? forLess = null)
        {
            Game.RequestToDoubleDown(this, forLess ?? Wager);
        }
    }
}
