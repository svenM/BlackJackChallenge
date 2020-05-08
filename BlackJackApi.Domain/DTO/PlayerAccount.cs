
namespace BlackJackApi.Domain.DTO
{
    public class PlayerAccount : IPlayerAccount
    {
        public PlayerAccount()
        {

        }
        public string Id { get; set; }
        public double Balance { get; set; }

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