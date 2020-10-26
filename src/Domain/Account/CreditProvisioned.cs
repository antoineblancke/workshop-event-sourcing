using System;

using Domain.Common;

namespace Domain.Account
{
    public class CreditProvisioned : Event
    {
        public readonly int NewCreditBalance;
        public readonly int CreditAmountProvisioned;

        public CreditProvisioned(string bankAccountId, int newCreditBalance, int creditAmountProvisioned) : base(bankAccountId)
        {
            NewCreditBalance = newCreditBalance;
            CreditAmountProvisioned = creditAmountProvisioned;
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }

        protected bool Equals(CreditProvisioned other)
        {
            return NewCreditBalance == other.NewCreditBalance && CreditAmountProvisioned == other.CreditAmountProvisioned;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))return false;
            if (ReferenceEquals(this, obj))return true;
            if (obj.GetType() != this.GetType())return false;
            return Equals((CreditProvisioned)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NewCreditBalance, CreditAmountProvisioned);
        }
    }
}
