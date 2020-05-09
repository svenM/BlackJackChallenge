namespace AdditionalDebugging
{
    using IO.Swagger.Api;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for toolControl.
    /// </summary>
    public partial class toolControl : UserControl
    {
        private static LobbyApi _lobbyClient;
        private static GameApi _gameClient;
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

        private void mnuNewDebugging_Click(object sender, RoutedEventArgs e)
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
    }
}