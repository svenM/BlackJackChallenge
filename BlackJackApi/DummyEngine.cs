using BlackJackApi.Domain;
using Stateless;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi
{
    public class DummyEngine
    {
        private BlackjackGameDealer _dealer;
        private List<BlackjackGamePlayer> _players;
        private List<BlackjackGameRoundPlayer> _roundPlayersQueuedForNextRound;

        private BlackjackGameRound _roundInProgress { get { return _dealer.RoundInProgress; } }

        public BlackjackHand DealerHand { get { return _roundInProgress?.DealerHand; } }
        public IEnumerable<BlackjackGamePlayer> Players { get { return _players.ToList(); } }

        public int MaxPlayers { get; private set; }

        public double MinWager { get; private set; }
        public double MaxWager { get; private set; }

    }
}
