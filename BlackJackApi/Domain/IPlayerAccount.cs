using System;

namespace BlackJackApi.Domain
{
    public interface IPlayerAccount
    {
        string Id { get; }
        double Balance { get; }
        void Credit(double amount);
        void Debit(double amount);
    }
}
