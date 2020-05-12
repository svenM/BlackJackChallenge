using BlackJackApi;
using BlackJackApi.Domain;
using BlackJackApi.Domain.DTO;
using System;
using System.Linq;
using System.Text;

namespace BlackJackApiConsoleTest
{
    class ProgramOld
    {
        static void Main(string[] args)
        {
            var decider = new PlayerActionDecider();
            var wager = 20;
            decider.Initialize();
            const double start = 5000;
            double balance = start;
            var rounds = 0;
            var game = new BlackjackGame(10, 100, 2);
            var account = new PlayerAccount(
                    id: Guid.NewGuid().ToString(),
                    startingBalance: balance);
            var alias = "P1";
            game.AddPlayer(account, alias);
            var player = game.Players.First(p => p.Alias == alias);
            StringBuilder round = new StringBuilder();

            while (balance > 0 && balance < start * 10)
            {
                round.AppendLine("-----------------NEW ROUND-----------------");
                rounds++;
                // Console.WriteLine("-----NEW ROUND-----");

                round.AppendLine("Wagering");
                player.SetWager(wager);
                round.AppendLine("refreshshoeifneeded");
                game.RefreshShoeIfNeeded();
                // Console.WriteLine("Start Round");
                game.StartRound();
                if (game.RoundClosedBecauseOfDealer21)
                {
                    round.AppendLine("dealer has 21");
                    //Console.WriteLine("Dealer has 21");
                    round.AppendLine("endround");
                    game.EndRound();
                    balance = player.Account.Balance;
                    Console.WriteLine(player.Account.Balance);
                    continue;
                }
                //Console.WriteLine(player.Hand.Cards.Count());
                ShowHand(player);

                var done = false;
                while (!done)
                {
                    if (player.Hand.IsBusted)
                    {
                        //  Console.WriteLine("Busted");
                        round.AppendLine("Busted!");
                        done = true;
                    }
                    else if (player.Hand.IsBlackjack || player.Hand.ScoreHighLow.High == 21)
                    {
                        //  Console.WriteLine($"standing with score {player.Hand.ScoreHighLow.High}");
                        round.AppendLine("player has blackjack");
                        done = true;
                        round.AppendLine("standing with bj");
                        player.Stand();
                    }
                    else
                    {
                        var action = decider.DecideAction(player.Hand, game.DealerHand.Cards.First());
                        switch (action)
                        {
                            case PlayerAction.Stand:
                                round.AppendLine("standing");
                                player.Stand();
                                done = true;
                                break;
                            case PlayerAction.Hit:
                                round.AppendLine("hit");
                                player.Hit();
                                break;
                            case PlayerAction.DoubleHit:
                                if (player.Hand.Cards.Count() == 2)
                                {
                                    round.AppendLine("doubledown");
                                    DoubleDown(wager, player);
                                    done = true;
                                }
                                else
                                {
                                    player.Hit();
                                }
                                break;
                            case PlayerAction.DoubleStand:
                                if (player.Hand.Cards.Count() == 2)
                                {
                                    round.AppendLine("doubledown2");
                                    DoubleDown(wager, player);
                                }
                                else
                                {
                                    player.Stand();
                                }
                                done = true;
                                break;
                            case PlayerAction.Split:
                                round.AppendLine("splithit");
                                player.Hit();
                                break;
                            case PlayerAction.SplitIfDAS:
                                round.AppendLine("splitdas");
                                player.Hit();
                                break;
                            case PlayerAction.DontSplit:
                                round.AppendLine("don'tsplit");
                                player.Hit();
                                break;
                            default:
                                break;
                        }
                    }
                }
                round.AppendLine("settling hand");
                game.SettlePlayerHand(player);

                round.AppendLine("endround");
                game.EndRound();
                balance = player.Account.Balance;
                if (balance > start)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine(player.Account.Balance);
            }
            Console.WriteLine($"{(balance <= 0 ? "Bankrupt" : "Rich")} after {rounds} rounds");
            Console.ReadKey();

        }

        private static void DoubleDown(int wager, BlackjackGamePlayer player)
        {
            if (player.Account.Balance >= wager)
            {
                player.DoubleDown(wager);
            }
            else
            {
                player.DoubleDown(player.Account.Balance);
            }
        }

        private static void ShowHand(BlackjackGamePlayer player)
        {
          /*  Console.WriteLine($"Score : {player.Hand.ScoreHighLow.Item1} - {player.Hand.ScoreHighLow.Item2}");
            foreach (var card in player.Hand.Cards)
            {
                Console.WriteLine($"card {card.Suit} {card.Rank}");
            }*/
        }
    }
}
