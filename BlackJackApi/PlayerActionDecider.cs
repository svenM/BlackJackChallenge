using BlackJackApi.Domain;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi
{
    public class PlayerActionDecider
    {
        Dictionary<(int playerTotal, CardRank dealerCard),  PlayerAction> Hard;
        Dictionary<(CardRank playerCard, CardRank dealerCard), PlayerAction> Soft;
        Dictionary<(CardRank card1, CardRank card2, CardRank dealerCard), PlayerAction> Split;

        public void Initialize()
        {
            var fi = new FileInfo("strategy/strategy.xlsx");
            using (var p = new ExcelPackage(fi))
            {
                
                var sheet = p.Workbook.Worksheets["Hard"];
                Hard = new Dictionary<(int playerTotal, CardRank dealerCard), PlayerAction>();
                var row = 2;
                var col = 2;
                while(!sheet.CellEmpty(row, 1))
                {
                    while (!sheet.CellEmpty(1, col))
                    {
                        var playerTotal = sheet.CellIntValue(row, 1);
                        var dealerHand = sheet.CellCardRank(1, col);
                        var action = sheet.CellPlayerAction(row, col);
                        if (dealerHand != null && action != null)
                        {
                            Hard.Add((playerTotal, dealerHand.Value), action.Value);
                        }
                        col++;
                    }
                    col = 2;
                    row++;
                }

                sheet = p.Workbook.Worksheets["Soft"];
                Soft = new Dictionary<(CardRank playerCard, CardRank dealerCard), PlayerAction>();
                row = 2;
                col = 3;
                while (!sheet.CellEmpty(row, 1))
                {
                    while (!sheet.CellEmpty(1, col))
                    {
                        var playerCard = sheet.CellCardRank(row, 2);
                        var dealerHand = sheet.CellCardRank(1, col);
                        var action = sheet.CellPlayerAction(row, col);

                        if (playerCard != null && dealerHand != null && action != null)
                        {
                            Soft.Add((playerCard.Value, dealerHand.Value), action.Value);
                        }
                        col++;
                    }
                    col = 3;
                    row++;
                }
            }
        
        }


        public PlayerAction DecideAction(BlackjackHand hand, Card dealerCard)
        {
            if (Hard == null) throw new Exception("Not initialized");
            if (hand.IsBusted) return PlayerAction.Stand;
            if (hand.ScoreHighLow.High == 21) return PlayerAction.Stand;

            if(hand.IsSoft)
            {
                if(hand.Cards.Count() == 2)
                {
                    var otherCard = hand.Cards.Where(c => c.Rank != CardRank.Ace);
                    if(otherCard.Any())
                    {
                        return Soft[(otherCard.First().Rank, dealerCard.Rank)];
                    }
                    else
                    {
                        // 2 aces => splitting later
                        return PlayerAction.Hit;//Hard[(2, dealerCard.Rank)];
                    }
                }
                else
                {
                    //get the total of the other cards
                    var total = hand.ScoreHighLow.Low - 1;
                    if(Enum.IsDefined(typeof(CardRank), total))
                    {
                        var rank = (CardRank)total;
                        return Soft[(rank, dealerCard.Rank)];
                    }
                    else
                    {
                        // not sure
                        return PlayerAction.Stand;
                    }
                }
            }
            else
            {
                return Hard[(hand.Score, dealerCard.Rank)];
            }
        }




    }
}
