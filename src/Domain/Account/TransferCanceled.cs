using System;
using domain.common;

namespace domain.account
{
    public class TransferCanceled : Event
    {
        public readonly string TransferId;

        public readonly string BankAccountIdDestination;

        public readonly int NewCreditBalance;

        public readonly int CreditRefund;

        public TransferCanceled(string bankAccountId, string transferId, string bankAccountIdDestination,
            int newCreditBalance, int creditRefund) : base(bankAccountId)
        {
            TransferId = transferId;
            BankAccountIdDestination = bankAccountIdDestination;
            NewCreditBalance = newCreditBalance;
            CreditRefund = creditRefund;
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }

        protected bool Equals(TransferCanceled other)
        {
            return TransferId == other.TransferId && BankAccountIdDestination == other.BankAccountIdDestination &&
                   NewCreditBalance == other.NewCreditBalance && CreditRefund == other.CreditRefund;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransferCanceled) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TransferId, BankAccountIdDestination, NewCreditBalance, CreditRefund);
        }
    }
}