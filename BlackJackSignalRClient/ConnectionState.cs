using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackSignalRClient
{
    public class ConnectionState
    {
        public bool GameHubConnected { get; set; } = false;
        public bool LobbyHubConnected { get; set; } = false;
        public bool GameHubError { get; set; } = false;
        public bool LobbyHubError{ get; set; } = false;
        public bool Faulted => GameHubError || LobbyHubError;
        public bool Ready => GameHubConnected && LobbyHubConnected;
    }
}
