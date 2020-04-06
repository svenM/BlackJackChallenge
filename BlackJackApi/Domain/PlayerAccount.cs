
namespace BlackJackApi.Domain
{
    public class PlayerAccount : IPlayerAccount
    {
        public string Id { get; private set; }
        public double Balance { get; private set; }

        public PlayerAccount(string id, double startingBalance)
        {
            Id = id;
            Balance = startingBalance;
        }

        public void Credit(double amount)
        {
            Balance += amount;
        }

        public void Debit(double amount)
        {
            Balance -= amount;
        }
    }
}