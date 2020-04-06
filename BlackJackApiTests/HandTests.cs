using BlackJackApi.DTO;
using System;
using Xunit;

namespace BlackJackApiTests
{
    public class HandTests
    {
        [Fact]
        public void AceCorrectValue()
        {
            //1 ace 
            var hand = new Hand(new Card(CardRank.Ace,CardSuit.Club));
            var value = hand.HandValue;

            Assert.Equal(11, value.NumericValue);
            Assert.True(value.IsSoft);
            Assert.False(value.Busted);

            //2 aces 
            hand.Cards.Add(new Card(CardRank.Ace, CardSuit.Diamond));
            value = hand.HandValue;

            Assert.Equal(12, value.NumericValue);
            Assert.True(value.IsSoft);
            Assert.False(value.Busted);

            //3 aces 
            hand.Cards.Add(new Card(CardRank.Ace, CardSuit.Heart));
            value = hand.HandValue;

            Assert.Equal(13, value.NumericValue);
            Assert.True(value.IsSoft);
            Assert.False(value.Busted);

            //4 aces 
            hand.Cards.Add(new Card(CardRank.Ace, CardSuit.Spade));
            value = hand.HandValue;

            Assert.Equal(14, value.NumericValue);
            Assert.True(value.IsSoft);
            Assert.False(value.Busted);

            //4 aces 
            hand.Cards.Add(new Card(CardRank.Ace, CardSuit.Spade));
            value = hand.HandValue;

            Assert.Equal(14, value.NumericValue);
            Assert.True(value.IsSoft);
        }
    }
}
