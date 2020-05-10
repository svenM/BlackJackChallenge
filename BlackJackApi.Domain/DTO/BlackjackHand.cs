using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackApi.Domain.DTO
{
    public class BlackjackHand
    {
        private IEnumerable<Card> _cards;

        public BlackjackHand()
        {
        }
        public IEnumerable<Card> Cards 
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;

                CalculateScore();
            }
        }

        private void CalculateScore()
        {
            ScoreHighLow = CalculateScoreHighLow();

            Score = ScoreHighLow.High > 21 ? ScoreHighLow.Low : ScoreHighLow.High;

            IsBlackjack = Cards.Count() == 2 && Score == 21;
            IsBusted = Score > 21;
            IsSoft = Cards.Any(a => a.Rank == CardRank.Ace);
        }

        [BsonIgnore]
        public (int Low, int High) ScoreHighLow { get; private set; }
        [BsonIgnore]
        public int Score { get; private set; }
        [BsonIgnore]
        public bool IsBlackjack { get; private set; }
        [BsonIgnore]
        public bool IsBusted { get; private set; }
        [BsonIgnore]
        public bool IsSoft { get; private set; }

        public BlackjackHand(IEnumerable<Card> cards) : this()
        {
            Cards = cards.ToList();
        }

        private (int Low, int High) CalculateScoreHighLow()
        {
            var aceCount = 0;
            var score = 0;

            foreach (var card in Cards)
            {
                if (card.Rank == CardRank.Ace)
                {
                    aceCount++;
                }
                else if (card.Rank >= CardRank.Ten)
                {
                    score += 10;
                }
                else
                {
                    score += (int)card.Rank;
                }
            }

            if (aceCount == 0)
                return (score, score);

            var LowScore = score + aceCount;
            var HighScore = LowScore + 10;

            return (LowScore, HighScore);
        }
    }

}
