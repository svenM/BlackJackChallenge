using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi.Domain
{

    public class Card
    {
        public Card()
        {

        }
        public CardRank Rank { get; set; }
        public CardSuit Suit { get; set; }
        public int NumericValue
        {
            get
            {
                var intValue = (int)Rank;
                return intValue > 10 ? 10 : intValue;
            }
        }

        public Card(CardRank rank)
        {
            Rank = rank;
            Suit = RandomMethods.RandomEnumValue<CardSuit>();
        }

        public Card(CardRank rank, CardSuit suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}
