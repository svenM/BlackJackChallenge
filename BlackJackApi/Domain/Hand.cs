using BlackJackApi.Domain;
using System.Collections.Generic;

namespace BlackJackApi.DTO
{
    public class Hand
    {
        public List<Card> Cards { get; set; } = new List<Card>();
        public Value HandValue
        {
            get
            {
                var value = new Value();
                var aceCount = 0;
                foreach (var card in Cards)
                {
                    if (card.Rank == CardRank.Ace)
                    {
                        value.IsSoft = true;
                        aceCount++;
                        value.NumericValue += 1;
                    }
                    else
                    {
                        value.NumericValue += card.NumericValue;
                    }
                }

                //check if we can increase
                while(value.NumericValue < 12 && aceCount > 0)
                {
                    value.NumericValue += 10;
                    aceCount--;
                }

                if (value.NumericValue > 21) value.Busted = true;
                return value;
            }
        }


        public Hand(Card firstCard)
        {
            Cards.Add(firstCard);
        }

        public Hand(params CardRank[] ranks)
        {
            foreach (var rank in ranks)
            {
                Cards.Add(new Card(rank));
            }
        }

        

    }
}
