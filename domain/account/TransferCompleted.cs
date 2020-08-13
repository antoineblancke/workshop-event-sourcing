using System;
using domain.common;

namespace domain.account
{
    public class TransferCompleted : Event
    {
        public readonly string TransferId;

        public readonly string BankAccountDestinationId;
        
        public TransferCompleted(string bankAccountId, string transferId, string bankAccountDestinationId) : base(bankAccountId)
        {
            TransferId = transferId;
            BankAccountDestinationId = bankAccountDestinationId;
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }

        protected bool Equals(TransferCompleted other)
        {
            return TransferId == other.TransferId && BankAccountDestinationId == other.BankAccountDestinationId;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransferCompleted) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TransferId, BankAccountDestinationId);
        }
    }
}