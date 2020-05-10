namespace AdditionalDebugging
{
    using IO.Swagger.Api;
    using IO.Swagger.Model;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for toolControl.
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

            var games = _lobbyClient.ApiLobbyListGet();
            lstOutput.Items.Clear();

            foreach (var game in games)
            {
                lstOutput.Items.Add(game.Name);
            }

        }

        private void mnuBet_Click(object sender, RoutedEventArgs e)
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

            mnuBet.IsEnabled = false;
            mnuCreateJoin.IsEnabled = false;
            mnuHit.IsEnabled = true;
            mnuDoubleDown.IsEnabled = true;
            mnuStand.IsEnabled = true;
            mnuQuit.IsEnabled = true;
            mnuList.IsEnabled = false;
        }

        private void mnuHit_Click(object sender, RoutedEventArgs e)
        {
            _gameClient.GameIdRequestPlayerIdRequestPost(_gameId, _playerId, "hit");
            var game = _gameClient.GameIdDetailsGet(_gameId);
            DrawGame(game);
        }

        private void mnuStand_Click(object sender, RoutedEventArgs e)
        {
            _gameClient.GameIdRequestPlayerIdRequestPost(_gameId, _playerId, "stand");
            var game = _gameClient.GameIdDetailsGet(_gameId);

            var outCome1 = game.RoundInProgressSettlements.First(p => p.PlayerId == _playerId);

            lstOutput.Items.Clear();
            lstOutput.Items.Add("Debug result = " + (outCome1.WagerOutcome == WagerOutcome.Win ? "++" : (outCome1.WagerOutcome == WagerOutcome.Lose ? "--" : "0")));

            mnuBet.IsEnabled = true;
            mnuCreateJoin.IsEnabled = false;
            mnuHit.IsEnabled = false;
            mnuDoubleDown.IsEnabled = false;
            mnuStand.IsEnabled = false;
            mnuQuit.IsEnabled = true;
            mnuList.IsEnabled = false;

        }

        private void mnuDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            _gameClient.GameIdRequestPlayerIdRequestPost(_gameId, _playerId, "doubledown");
            var game = _gameClient.GameIdDetailsGet(_gameId);
            DrawGame(game);
        }

        private void mnuCreateJoin_Click(object sender, RoutedEventArgs e)
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

            mnuBet.IsEnabled = false;
            mnuCreateJoin.IsEnabled = false;
            mnuHit.IsEnabled = true;
            mnuDoubleDown.IsEnabled = true;
            mnuStand.IsEnabled = true;
            mnuQuit.IsEnabled = true;
            mnuList.IsEnabled = false;
        }

        private void DrawGame(LiveBlackjackGame game)
        {
            lstOutput.Items.Clear();
            string alias;
            string rank;

            if (game.DealerHas21.HasValue && game.DealerHas21.Value)
            {
                lstOutput.Items.Add("Server has won condition. Please retry with button B");
                mnuBet.IsEnabled = true;
                mnuCreateJoin.IsEnabled = false;
                mnuHit.IsEnabled = false;
                mnuDoubleDown.IsEnabled = false;
                mnuStand.IsEnabled = false;
                mnuQuit.IsEnabled = true;
                mnuList.IsEnabled = false;
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
            }

            var dealerCard = game.DealerHand.Cards.First();
            alias = TranslateSuit(dealerCard);
            rank = TranslateRank(dealerCard.Rank);
            lstOutput.Items.Add($"Warning : {alias} at {rank}");

            lstOutput.Items.Add($"Possible {(playerHand.IsSoft.HasValue && playerHand.IsSoft.Value ? "soft" : "hard")} errors around line {(playerHand.Score.HasValue ? playerHand.Score.Value : 0)}");
        }

        private static string TranslateSuit(Card card)
        {
            switch (card.Suit)
            {
                case CardSuit.NUMBER_0:
                    return  "DevideByZeroException";
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
            if((int)rank <= 10)
            {
                return  "line " + ((int)rank).ToString();
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