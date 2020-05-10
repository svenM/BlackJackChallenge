using BlackJackApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackApi.Domain.DTO
{
    public class BlackjackGameRound
    {
        public BlackjackGameRound()
        {

        }
        public List<BlackjackGameRoundPlayer> RoundPlayers { get; set; }

        public List<Card> DealerCards { get; set; }
        public BlackjackHand DealerHand { get; set; }

        public List<BlackjackHandSettlement> Settlements { get; set;  }

        public bool IsInitialized { get; set; }
        public bool DealerHas21 { get; set; } = false;

        public BlackjackGameRound(IEnumerable<BlackjackGameRoundPlayer> roundplayers)
        {
            if (roundplayers == null || !roundplayers.Any())
                throw new InvalidOperationException("At least one player required to create a new round");

            DealerCards = new List<Card>();
            Settlements = new List<BlackjackHandSettlement>();

            RoundPlayers = roundplayers.OrderBy(a => a.Player.Position).ToList();
            RoundPlayers.ForEach(player =>
            {
                player.HasAction = false;
            });

            IsInitialized = false;
        }

        public void InitializeAction()
        {
            if (IsInitialized)
                return;

            if (RoundPlayers.Any(a => a.HasAction))
                throw new InvalidOperationException();

            if (RoundPlayers.Any())
                RoundPlayers.First().HasAction = true;

            IsInitialized = true;
        }

        public bool MoveToNextAction()
        {
            var currentPlayerWithAction = RoundPlayers.SingleOrDefault(a => a.HasAction);
            if (currentPlayerWithAction == null)
                return false;

            currentPlayerWithAction.HasAction = false;

            var nextPlayerWithAction = RoundPlayers
                .Where(a => a.Player.Position > currentPlayerWithAction.Player.Position)
                .OrderBy(a => a.Player.Position)
                .FirstOrDefault();

            if (nextPlayerWithAction == null)
                return false;

            nextPlayerWithAction.HasAction = true;
            return true;
        }

        public void AddCardToDealerHand(Card card)
        {
            if (card != null)
            {
                DealerCards.Add(card);
                if (DealerCards.Count() >= 2)
                {
                    DealerHand = new BlackjackHand(DealerCards);

                    if (DealerHand.IsBlackjack || DealerHand.Score == 21)
                    {
                        DealerHas21 = true;
                    }
                }
            }
        }

        public BlackjackGameRoundPlayer GetRoundPlayer(BlackjackGamePlayer player)
        {
            return RoundPlayers.SingleOrDefault(a => a.Player.Id == player.Id);
        }

        public void SettleRoundPlayer(BlackjackGameRoundPlayer roundplayer, BlackjackHandSettlement settlement)
        {
            Settlements.Add(settlement);
            RoundPlayers.Remove(roundplayer);
        }
    }
}
