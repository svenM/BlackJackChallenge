using System;
using NUnit.Framework;
using BlackJackApi.Domain.DTO;

namespace BlackJackApiTests
{
    public class HandTests
    {
        [Test]
        public void AceCorrectValue([Range(1, 100)] int aces)
        {
            //Determine Expected
            var expectedValue = (aces > 11) ? aces : aces + 10;
            var busted = expectedValue > 21;

            var hand = new Hand(new Card(CardRank.Ace, CardSuit.Club));

            for (int i = 0; i < aces - 1; i++)
            {
                hand.Cards.Add(new Card(CardRank.Ace, CardSuit.Club));
            }

            var value = hand.HandValue;

            Assert.AreEqual(expectedValue, value.NumericValue, "Numeric value not equal to expected");
            Assert.True(value.IsSoft, "IsSoft should be true");
            Assert.AreEqual(busted, value.Busted, $"Busted should be {busted}");

        }

        [TestCase(CardRank.Ten, CardRank.Eight, 18, false, false)]
        [TestCase(CardRank.Ace, CardRank.Two, 13, true, false)]
        [TestCase(CardRank.King, CardRank.King, 20, false, false)]
        [TestCase(CardRank.Ace, CardRank.King, 21, true, false)]
        public void SteekProef(CardRank c1, CardRank c2, int expectedValue, bool soft, bool busted)
        {
            var hand = new Hand(c1, c2);
            var value = hand.HandValue;
            Assert.AreEqual(expectedValue, value.NumericValue);
            Assert.AreEqual(soft, value.IsSoft);
            Assert.AreEqual(busted, value.Busted, "Should not be busted");

        }

        [Test]
        public void TestTwoNormalCards([Range(2, 13)] int c1, [Range(2, 13)] int c2)
        {
            var expectedValue = (c1 > 10 ? 10 : c1) + (c2 > 10 ? 10 : c2);

            var hand = new Hand((CardRank)c1, (CardRank)c2);
            var value = hand.HandValue;
            Assert.AreEqual(expectedValue, value.NumericValue, "NumericValue unexpected");
            Assert.False(value.IsSoft, "Should not be soft as there are no aces");
            Assert.False(value.Busted, "Should not be busted");
        }

        [Test]
        public void TestThreeNormalCards([Range(2, 13)] int c1, [Range(2, 13)] int c2, [Range(2, 13)] int c3)
        {
            var expectedValue = (c1 > 10 ? 10 : c1) + (c2 > 10 ? 10 : c2) + (c3 > 10 ? 10 : c3);
            var busted = expectedValue > 21;

            var hand = new Hand((CardRank)c1, (CardRank)c2, (CardRank)c3);
            var value = hand.HandValue;
            Assert.AreEqual(expectedValue, value.NumericValue, "NumericValue unexpected");
            Assert.False(value.IsSoft, "Should not be soft as there are no aces");
            Assert.AreEqual(busted,value.Busted, "Busted value unexpected");
        }

        /*  
         *  This does not make nunit very happy: 20736 testcases. Enable at own risk
          [Test]*/
        public void TestFourNormalCards([Range(2, 13)] int c1, [Range(2, 13)] int c2, [Range(2, 13)] int c3, [Range(2, 13)] int c4)
        {
            var expectedValue = (c1 > 10 ? 10 : c1) + (c2 > 10 ? 10 : c2) + (c3 > 10 ? 10 : c3) + (c4 > 10 ? 10 : c4);
            var busted = expectedValue > 21;

            var hand = new Hand((CardRank)c1, (CardRank)c2, (CardRank)c3, (CardRank)c4);
            var value = hand.HandValue;
            Assert.AreEqual(expectedValue, value.NumericValue, "NumericValue unexpected");
            Assert.False(value.IsSoft, "Should not be soft as there are no aces");
            Assert.AreEqual(busted, value.Busted, "Busted value unexpected");
        }
        
    }
}
