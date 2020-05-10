using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackApi.Domain.DTO
{
    public class BlackjackGameRoundPlayer
    {
        public BlackjackGameRoundPlayer()
        {

        }
        public List<Card> Cards { get; set; }

        public BlackjackGamePlayer Player { get; set; }
        public BlackjackHand Hand { get; set; }
        public double Wager { get; set; }
        public bool HasAction { get; set; }        

        public BlackjackGameRoundPlayer(BlackjackGamePlayer player, double wager)
        {
            if (player == null)
                throw new ArgumentNullException("player", "Player is null");

            if (wager <= 0)
                throw new InvalidOperationException("Wager cannot be negative");

            Cards = new List<Card>();
            Player = player;
            Wager = wager;
        }

        public void AddCardToHand(Card card)
        {
            if (card != null)
            {
                Cards.Add(card);
                if (Cards.Count() >= 2)
                    Hand = new BlackjackHand(Cards);
            }
        }
    }
}
