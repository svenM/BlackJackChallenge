using System;

namespace BlackJackApi.Domain
{
    public class BlackjackHandSettlement
    {
        public BlackjackHandSettlement()
        {

        }
        public int PlayerPosition { get; set; }
        public BlackjackHand PlayerHand { get; set; }
        public BlackjackHand DealerHand { get; set; }
        public WagerOutcome WagerOutcome { get; set; }
        public double WagerAmount { get; set; }

        public BlackjackHandSettlement(BlackjackGameRoundPlayer player, BlackjackHand dealerHand)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            if (player.Hand == null)
                throw new ArgumentNullException("player.hand");

            if (dealerHand == null)
                throw new ArgumentNullException("dealerHand");

            DealerHand = dealerHand;
            PlayerHand = player.Hand;            
            PlayerPosition = player.Player.Position;
            WagerAmount = player.Wager;
            WagerOutcome = GetOutcome();            
        }

        private WagerOutcome GetOutcome()
        {
            var score = PlayerHand.Score;
            var dealerScore = DealerHand.Score;

            if (score > 21)
                return WagerOutcome.Lose;

            if (dealerScore > 21)
                return WagerOutcome.Win;

            if (score == dealerScore)
                return WagerOutcome.Draw;

            return score > dealerScore ? WagerOutcome.Win : WagerOutcome.Lose;
        }
    }
}
