using System;
using domain.common;

namespace domain.account
{
    public class CreditWithdrawn : Event
    {
        public readonly int NewCreditBalance;
        public readonly int CreditAmountWithdrawn;
        
        public CreditWithdrawn(string bankAccountId, int newCreditBalance, int creditAmountWithdrawn) : base(bankAccountId)
        {
            NewCreditBalance = newCreditBalance;
            CreditAmountWithdrawn = creditAmountWithdrawn;
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }

        protected bool Equals(CreditWithdrawn other)
        {
            return NewCreditBalance == other.NewCreditBalance && CreditAmountWithdrawn == other.CreditAmountWithdrawn;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CreditWithdrawn) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NewCreditBalance, CreditAmountWithdrawn);
        }
    }
}