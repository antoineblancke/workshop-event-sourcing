using System;

using Domain.Common;

namespace Domain.Account
{
    public class TransferReceived : Event
    {
        public readonly string TransferId;

        public readonly string BankAccountOriginId;

        public readonly int NewCreditBalance;

        public readonly int CreditTransferred;

        public TransferReceived(string bankAccountId, string transferId, string bankAccountOriginId,
            int newCreditBalance, int creditTransferred) : base(bankAccountId)
        {
            TransferId = transferId;
            BankAccountOriginId = bankAccountOriginId;
            NewCreditBalance = newCreditBalance;
            CreditTransferred = creditTransferred;
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }

        protected bool Equals(TransferReceived other)
        {
            return TransferId == other.TransferId && BankAccountOriginId == other.BankAccountOriginId &&
                NewCreditBalance == other.NewCreditBalance && CreditTransferred == other.CreditTransferred;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))return false;
            if (ReferenceEquals(this, obj))return true;
            if (obj.GetType() != this.GetType())return false;
            return Equals((TransferReceived)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TransferId, BankAccountOriginId, NewCreditBalance, CreditTransferred);
        }
    }
}
