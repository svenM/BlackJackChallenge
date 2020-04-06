using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi.Domain
{
    public enum GameState
    {
        NoGame = 0,
        PlayerBetting = 1,
        PlayerPlaying = 2,
        DealerPlaying =3,
        Results = 4,
        DealingPlayerCards = 5
    }
}
