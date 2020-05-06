using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackApi.Domain
{
    public class BlackjackGame
    {
        public List<BlackjackGameRoundPlayer> RoundPlayersQueuedForNextRound { get; set;  }

        private BlackjackGameRound _roundInProgress { get { return Dealer.RoundInProgress; } }

        public BlackjackHand DealerHand { get { return _roundInProgress?.DealerHand; } }

        public bool DealerHas21
        {
            get
            {
                if (_roundInProgress != null)
                {
                    return _roundInProgress.DealerHas21;
                }
                else
                {
                    return false;
                }
            }
        }
        public List<BlackjackGamePlayer> Players { get; set; }
        public BlackjackGameDealer Dealer { get; set; }

        public int MaxPlayers { get; private set; }

        public double MinWager { get; private set; }
        public double MaxWager { get; private set; }

        public bool IsRoundInProgress { get { return _roundInProgress != null; } }
        public int PercentRemainingInDealerShoe { get { return Dealer.PercentRemainingInShoe; } }

        public IEnumerable<BlackjackHandSettlement> RoundInProgressSettlements
        {
            get
            {
                return _roundInProgress?.Settlements?.ToList() ??
                    Enumerable.Empty<BlackjackHandSettlement>();
            }
        }

        public BlackjackGame(double minWager, double maxWager, int maxPlayers = 6)
        {
            if (minWager <= 0)
                throw new InvalidOperationException("Min wager must be greater than 0");

            if (minWager > maxWager)
                throw new InvalidOperationException("Max wager must be greater than or equal to min wager");

            if (maxPlayers < 1)
                throw new InvalidOperationException("Game must accommodate at least 1 player");

            Dealer = new BlackjackGameDealer();
            Players = new List<BlackjackGamePlayer>(MaxPlayers);
            RoundPlayersQueuedForNextRound = new List<BlackjackGameRoundPlayer>(MaxPlayers);
            MaxPlayers = maxPlayers;
            MinWager = minWager;
            MaxWager = maxWager;
        }

        public bool IsPositionOpen(int position)
        {
            return position > 0 && position <= MaxPlayers &&
                !Players.Any(a => a.Position == position);
        }

        public void AddPlayer(PlayerAccount account, string alias, int position)
        {
            if (account == null)
                throw new ArgumentNullException("account", "Account is null");

            if (Players.Count() >= MaxPlayers)
                throw new InvalidOperationException("Game is full");

            if (!IsPositionOpen(position))
                throw new InvalidOperationException("Position is not valid");

            if (account.Balance < MinWager)
                throw new InvalidOperationException("Insufficient player funds");

            Players.Add(new BlackjackGamePlayer(account, this, alias, position));
        }

        public void AddPlayer(PlayerAccount account, string alias)
        {
            int position = 1;
            while (position <= MaxPlayers && !IsPositionOpen(position))
                position++;

            AddPlayer(account, alias, position);
        }

        public void RemovePlayer(BlackjackGamePlayer player)
        {
            if (Players.Contains(player))
            {
                if (player.IsLive)
                    throw new InvalidOperationException("Player is in live round");

                Players.Remove(player);
                RoundPlayersQueuedForNextRound
                    .Remove(RoundPlayersQueuedForNextRound
                        .FirstOrDefault(a => a.Player.Id == player.Id));
            }
        }

        public void RefreshDealerShoe()
        {
            Dealer.RefreshShoe();
        }

        public void RefreshShoeIfNeeded()
        {
            if (PercentRemainingInDealerShoe < 20)
                RefreshDealerShoe();
        }
        public virtual void StartRound()
        {
            if (IsRoundInProgress)
                throw new InvalidOperationException("Live round in progress");

            if (!RoundPlayersQueuedForNextRound.Any())
                throw new InvalidOperationException("No players have wagered");

            var roundInProgress = new BlackjackGameRound(RoundPlayersQueuedForNextRound);
            RoundPlayersQueuedForNextRound.Clear();

            Dealer.Deal(roundInProgress);
        }

        public BlackjackHandSettlement SettlePlayerHand(BlackjackGamePlayer player)
        {
            return Dealer.SettleHand(player);
        }

        public virtual void EndRound()
        {
            Dealer.CloseRound();
        }

        internal void PlaceWager(BlackjackGamePlayer player, double amount)
        {
            if (!Players.Contains(player))
                throw new InvalidOperationException("'player' is null or invalid");

            if (player.IsLive)
                throw new InvalidOperationException("Player is in live round");

            if (RoundPlayersQueuedForNextRound.Any(a => a.Player.Id == player.Id))
                throw new InvalidOperationException();

            if (amount > player.Account.Balance)
                throw new InvalidOperationException("Insufficient funds");

            if (amount < MinWager || amount > MaxWager)
                throw new InvalidOperationException("Player wager is out of range");

            player.Account.Debit(amount);
            RoundPlayersQueuedForNextRound.Add(new BlackjackGameRoundPlayer(player, amount));
        }

        internal void RequestToHit(BlackjackGamePlayer player)
        {
            Dealer.HandleRequestToHit(player);
        }

        internal void RequestToStand(BlackjackGamePlayer player)
        {
            Dealer.HandleRequestToStand(player);
        }

        internal void RequestToDoubleDown(BlackjackGamePlayer player, double amount)
        {
            if (amount > player?.Account?.Balance)
                throw new InvalidOperationException("Insufficient funds");

            Dealer.HandleRequestToDoubleDown(player, amount);
        }

        internal bool GetPlayerIsLive(BlackjackGamePlayer player)
        {
            return _roundInProgress?.GetRoundPlayer(player) != null;
        }

        internal BlackjackHand GetPlayerHand(BlackjackGamePlayer player)
        {
            return _roundInProgress?.GetRoundPlayer(player)?.Hand;
        }

        internal double GetPlayerWager(BlackjackGamePlayer player)
        {
            return _roundInProgress?.GetRoundPlayer(player)?.Wager
                ?? RoundPlayersQueuedForNextRound.SingleOrDefault(a => a.Player.Id == player.Id)?.Wager
                ?? 0;
        }

        internal bool GetPlayerHasAction(BlackjackGamePlayer player)
        {
            return _roundInProgress?.GetRoundPlayer(player)?.HasAction ?? false;
        }
    }
}
