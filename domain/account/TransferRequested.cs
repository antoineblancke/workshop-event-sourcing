using System;
using domain.common;

namespace domain.account
{
    public class TransferRequested : Event
    {
        public readonly string BankAccountDestinationId;

        public readonly string TransferId;

        public readonly int NewCreditBalance;

        public readonly int CreditTransferred;

        public TransferRequested(string bankAccountId, string bankAccountDestinationId, string transferId,
            int newCreditBalance, int creditTransferred) : base(bankAccountId)
        {
            BankAccountDestinationId = bankAccountDestinationId;
            TransferId = transferId;
            NewCreditBalance = newCreditBalance;
            CreditTransferred = creditTransferred;
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }

        protected bool Equals(TransferRequested other)
        {
            return BankAccountDestinationId == other.BankAccountDestinationId && TransferId == other.TransferId &&
                   NewCreditBalance == other.NewCreditBalance && CreditTransferred == other.CreditTransferred;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransferRequested) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BankAccountDestinationId, TransferId, NewCreditBalance, CreditTransferred);
        }
    }
}