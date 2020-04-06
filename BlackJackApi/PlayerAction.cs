using BlackJackApi.Domain;

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