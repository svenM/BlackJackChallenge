namespace AdditionalDebugging
{
    using IO.Swagger.Api;
    using IO.Swagger.Model;
    using Microsoft.Win32;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for toolControl.
    /// clean C:\Users\svma\AppData\Local\Microsoft\VisualStudio experimental dir if not refreshing
    /// </summary>
    public partial class toolControl : UserControl
    {
        private static LobbyApi _lobbyClient;
        private static GameApi _gameClient;
        private static string _gameId;
        private static string _playerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="toolControl"/> class.
        /// </summary>
        public toolControl()
        {
            var url = "http://localhost/BlackJackApi/";
            this.InitializeComponent();
            //initialize connection
            _lobbyClient = new LobbyApi(url);
            _gameClient = new GameApi(url);
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }


        private void mnuList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var games = _lobbyClient.ApiLobbyListGet();
                lstOutput.Items.Clear();

                foreach (var game in games)
                {
                    lstOutput.Items.Add(game.Name);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void mnuBet_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                _gameClient.GameIdPlayerPlayerIdBetAmountPost(_gameId, _playerId, 10);

                var game = _gameClient.GameIdDetailsGet(_gameId);
                if (!game.IsRoundInProgress.HasValue || !game.IsRoundInProgress.Value)
                {
                    _gameClient.GameIdDealGet(_gameId);
                }
                // get the cards
                game = _gameClient.GameIdDetailsGet(_gameId);
                DrawGame(game);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void mnuHit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _gameClient.GameIdRequestPlayerIdRequestPost(_gameId, _playerId, "hit");
                var game = _gameClient.GameIdDetailsGet(_gameId);
                DrawGame(game);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void mnuStand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _gameClient.GameIdRequestPlayerIdRequestPost(_gameId, _playerId, "stand");
                var game = _gameClient.GameIdDetailsGet(_gameId);

                var outCome1 = game.RoundInProgressSettlements.First(p => p.PlayerId == _playerId);

                lstOutput.Items.Clear();
                lstOutput.Items.Add("Debug result = " + (outCome1.WagerOutcome == WagerOutcome.Win ? "++" : (outCome1.WagerOutcome == WagerOutcome.Lose ? "--" : "0")));
                string alias;
                string rank;
                foreach (var card in game.RoundInProgressSettlements.First().PlayerHand.Cards)
                {
                    alias = TranslateSuit(card);
                    rank = TranslateRank(card.Rank);
                    lstOutput.Items.Add($"Fixed possible error : {alias} at {rank}");
                }
                lstOutput.Items.Add("Total errors fixed : " + game.RoundInProgressSettlements.First().PlayerHand.Score);
                foreach (var c in game.RoundInProgressSettlements.First().DealerHand.Cards)
                {
                    alias = TranslateSuit(c);
                    rank = TranslateRank(c.Rank);
                    lstOutput.Items.Add($"Removed warning : {alias} at {rank}");
                }
                lstOutput.Items.Add("Total warnings removed : " + game.RoundInProgressSettlements.First().DealerHand.Score);


                _gameClient.GameIdEndPost(_gameId);
                SetRoundEndedButtons();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void mnuDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _gameClient.GameIdRequestPlayerIdRequestPost(_gameId, _playerId, "doubledown");
                var game = _gameClient.GameIdDetailsGet(_gameId);

                var outCome1 = game.RoundInProgressSettlements.First(p => p.PlayerId == _playerId);

                lstOutput.Items.Clear();
                lstOutput.Items.Add("Debug result = " + (outCome1.WagerOutcome == WagerOutcome.Win ? "++" : (outCome1.WagerOutcome == WagerOutcome.Lose ? "--" : "0")));
                string alias;
                string rank;
                foreach (var card in game.RoundInProgressSettlements.First().PlayerHand.Cards)
                {
                    alias = TranslateSuit(card);
                    rank = TranslateRank(card.Rank);
                    lstOutput.Items.Add($"Fixed possible error : {alias} at {rank}");
                }
                lstOutput.Items.Add("Total errors fixed: " + game.RoundInProgressSettlements.First().PlayerHand.Score);
                foreach (var c in game.RoundInProgressSettlements.First().DealerHand.Cards)
                {
                    alias = TranslateSuit(c);
                    rank = TranslateRank(c.Rank);
                    lstOutput.Items.Add($"Removed warning : {alias} at {rank}");
                }
                lstOutput.Items.Add("Total warnings removes : " + game.RoundInProgressSettlements.First().DealerHand.Score);

                _gameClient.GameIdEndPost(_gameId);
                SetRoundEndedButtons();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void mnuHint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // create a game and join it 
                var hint = _gameClient.GetHint(_gameId, _playerId).Data.Replace("\"","");
                if (string.IsNullOrWhiteSpace(hint)) return;
                switch (hint.ToLower().Trim())
                {
                    case "stand":
                        hint = "Suggested you stop debugging";
                        break;
                    case "hit":
                        hint = "Suggested you keep debugging";
                        break;
                    case "doublehit":
                        hint = "Suggested you double your debugging efforts";
                        break;
                    case "doublestand":
                        hint = "Suggested you double your debugging efforts";
                        break;
                    case "split":
                        hint = "Debugging has no hint";
                        break;
                    case "splitifdas":
                        hint = "Debugging has no hint";
                        break;
                    case "dontsplit":
                        hint = "Don't split debugging";
                        break;
                }
                lstOutput.Items.Add(hint);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void mnuCreateJoin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // create a game and join it 
                _gameId = _lobbyClient.ApiLobbyNewgameNameMinbetMaxbetPost($"ADDBG{DateTime.Now.ToString("yyyyMMddHHmm")}", 10, 100).Replace("\"", "");
                var model = _gameClient.GameIdJoinPut(_gameId, "Debugger", 1);
                _playerId = model.Id;

                // Bet on our first hand
                _gameClient.GameIdPlayerPlayerIdBetAmountPost(_gameId, _playerId, 10);

                // get the cards
                var game = _gameClient.GameIdDetailsGet(_gameId);
                DrawGame(game);
                SetRoundBusyButtons();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void SetRoundBusyButtons()
        {
            mnuBet.IsEnabled = false;
            mnuCreateJoin.IsEnabled = false;
            mnuHit.IsEnabled = true;
            mnuDoubleDown.IsEnabled = true;
            mnuHint.IsEnabled = true;
            mnuStand.IsEnabled = true;
            mnuQuit.IsEnabled = true;
            mnuList.IsEnabled = false;
        }

        private void DrawGame(LiveBlackjackGame game)
        {
            lstOutput.Items.Clear();
            string alias;
            string rank;
            mnuBalance.Header = game.Players.First().Account.Balance;
            if (game.DealerHas21.HasValue && game.DealerHas21.Value)
            {
                lstOutput.Items.Add("Server has won condition. Please retry with button B");
                SetRoundEndedButtons();
                return;
            }
            var playerHand = game.Players.First().Hand;

            if (playerHand != null && playerHand.Cards != null)
            {
                foreach (var card in playerHand.Cards)
                {
                    alias = TranslateSuit(card);
                    rank = TranslateRank(card.Rank);
                    lstOutput.Items.Add($"Possible error : {alias} at {rank}");
                }
                lstOutput.Items.Add($"Possible {(playerHand.IsSoft.HasValue && playerHand.IsSoft.Value ? "soft" : "hard")} errors around line {(playerHand.Score.HasValue ? playerHand.Score.Value : 0)}");
                var dealerCard = game.DealerHand.Cards.First();
                alias = TranslateSuit(dealerCard);
                rank = TranslateRank(dealerCard.Rank);
                lstOutput.Items.Add($"Warning : {alias} at {rank}");
                SetRoundBusyButtons();
            }

            if (game.RoundInProgressSettlements != null && game.RoundInProgressSettlements.Any())
            {
                try
                {
                    _gameClient.GameIdEndPost(_gameId);
                }
                catch (Exception)
                {
                }
                lstOutput.Items.Add("Debugging ended");
                foreach (var card in game.RoundInProgressSettlements.First().PlayerHand.Cards)
                {
                    alias = TranslateSuit(card);
                    rank = TranslateRank(card.Rank);
                    lstOutput.Items.Add($"Possible error : {alias} at {rank}");
                }
                lstOutput.Items.Add("Total errors : " + game.RoundInProgressSettlements.First().PlayerHand.Score);
                foreach (var c in game.RoundInProgressSettlements.First().DealerHand.Cards)
                {
                    alias = TranslateSuit(c);
                    rank = TranslateRank(c.Rank);
                    lstOutput.Items.Add($"Warning : {alias} at {rank}");
                }
                lstOutput.Items.Add("Total Warnings : " + game.RoundInProgressSettlements.First().DealerHand.Score);
                SetRoundEndedButtons();
            }


        }

        private void SetRoundEndedButtons()
        {
            mnuBet.IsEnabled = true;
            mnuCreateJoin.IsEnabled = false;
            mnuHit.IsEnabled = false;
            mnuDoubleDown.IsEnabled = false;
            mnuStand.IsEnabled = false;
            mnuQuit.IsEnabled = true;
            mnuList.IsEnabled = false; 
            mnuHint.IsEnabled = false;
        }

        private static string TranslateSuit(Card card)
        {
            switch (card.Suit)
            {
                case CardSuit.NUMBER_0:
                    return "DevideByZeroException";
                case CardSuit.NUMBER_1:
                    return "ObjectNotSetException";
                case CardSuit.NUMBER_2:
                    return "LogicalException";
                case CardSuit.NUMBER_3:
                    return "BusinessException";
                default:
                    return "NotFoundException";
            }
        }

        private string TranslateRank(CardRank? rank)
        {
            if ((int)rank <= 10)
            {
                return "line " + ((int)rank).ToString();
            }
            else // for J,Q,K return next Letter
            {
                switch (rank)
                {
                    case CardRank.NUMBER_11:
                        //J
                        return "region K";

                    case CardRank.NUMBER_12:
                        //Q 
                        return "region R";
                    case CardRank.NUMBER_13:
                        //K
                        return "region L";
                    default:
                        return "unknown region";
                }
            }
        }


        private void grd_LayoutUpdated(object sender, System.EventArgs e)
        {
            lstOutput.Width = grd.Width;
            lstOutput.Height = grd.Height - menuBar.Height;
        }
    }
}