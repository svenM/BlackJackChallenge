using BlackJackApi.Domain;
using BlackJackApi.Domain.DTO;

namespace BlackJackApi
{
    public enum PlayerAction
    {
        [ExcelValue("S")]
        Stand,
        [ExcelValue("H")]
        Hit,
        [ExcelValue("D")]
        DoubleHit,
        [ExcelValue("DS")]
        DoubleStand,
        [ExcelValue("Y")]
        Split,
        [ExcelValue("YD")]
        SplitIfDAS,
        [ExcelValue("N")]
        DontSplit
    }
}