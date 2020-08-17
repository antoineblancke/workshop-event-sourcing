using System.Collections.Concurrent;
using System.Collections.Generic;

namespace projection
{
    public class InMemoryBalanceRepository : IBalanceRepository
    {
        private readonly Dictionary<string, int?> balance = new Dictionary<string, int?>();
        
        public void WriteCreditBalance(string bankAccountId, int credit)
        {
            balance.Add(bankAccountId, credit);
        }

        public int? ReadBalance(string bankAccountId)
        {
            return balance.GetValueOrDefault(bankAccountId, null);
        }

        public void Clear()
        {
            balance.Clear();
        }
    }
}