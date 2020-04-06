namespace BlackJackApi.Domain
{
    public enum CardRank
    {
        [ExcelValue("A")]
        Ace = 1,
        [ExcelValue("2")]
        Two = 2,
        [ExcelValue("3")]
        Three = 3,
        [ExcelValue("4")]
        Four = 4,
        [ExcelValue("5")]
        Five = 5,
        [ExcelValue("6")]
        Six = 6,
        [ExcelValue("7")]
        Seven = 7,
        [ExcelValue("8")]
        Eight = 8,
        [ExcelValue("9")]
        Nine = 9,
        [ExcelValue("10")]
        Ten = 10,
        [ExcelValue("J")]
        Jack = 11,
        [ExcelValue("Q")]
        Queen = 12,
        [ExcelValue("K")]
        King = 13
    }
}
