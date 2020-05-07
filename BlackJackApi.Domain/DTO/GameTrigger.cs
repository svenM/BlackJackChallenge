using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi.Domain.DTO
{
    public enum GameTrigger
    {
        GameStarting,
        PlayerHasBet,
        PlayerHit,
        PlayerStop,
        DealerHit,
        DealerStop,
        DoneDealing
    }
}
