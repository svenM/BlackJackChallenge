namespace BlackJackApi.DTO
{
    public class Value
    {
        public int NumericValue { get; set; }
        public bool IsSoft { get; set; }
        public bool Busted { get; set; } = false;
    }
}
