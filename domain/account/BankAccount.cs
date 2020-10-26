using System;
using System.Collections.Generic;
using System.Linq;
using domain.common;

namespace domain.account
{
    public class BankAccount
    {
        public static BankAccount RegisterBankAccount(string bankAccountId, IEventStore eventStore)
        {
            BankAccount bankAccount = new BankAccount(eventStore);
            bankAccount.RegisterBankAccount(bankAccountId);
            return bankAccount;
        }

        public static BankAccount LoadBankAccount(string bankAccountId, IEventStore eventStore)
        {
            var events = eventStore.Load(bankAccountId);
            return events.Any() ? new BankAccount(bankAccountId, eventStore, events) : BankAccount.Empty;
        }

        private static readonly BankAccount Empty = new BankAccount(null);

        private readonly InnerEventListener eventProcessor;

        private string id;

        private int version;

        private IEventStore eventStore;

        private int creditBalance;

        private readonly IDictionary<string, TransferRequested> pendingTransfers;

        private BankAccount(IEventStore eventStore) : this(string.Empty, eventStore, new List<Event>())
        {
        }

        private BankAccount(string id, IEventStore eventStore, List<Event> events) : this(id, eventStore, 0, 0)
        {
            events.ForEach(this.ApplyEvent);
        }

        public BankAccount(string id, IEventStore eventStore, int creditBalance, int aggregateVersion) : this(id,
            eventStore, creditBalance, aggregateVersion, new Dictionary<string, TransferRequested>())
        {
        }

        public BankAccount(string id, IEventStore eventStore, int creditBalance, int aggregateVersion,
            IDictionary<string, TransferRequested> pendingTransfers)
        {
            this.id = id;
            this.eventStore = eventStore;
            this.creditBalance = creditBalance;
            this.version = aggregateVersion;
            this.pendingTransfers = pendingTransfers;
            this.eventProcessor = new InnerEventListener(this);
        }

        /* Decision Function */
        private void RegisterBankAccount(string bankAccountId)
        {
            var bankAccountRegisteredEvent = new BankAccountRegistered(bankAccountId);
            eventStore.Save(this.version, bankAccountRegisteredEvent).ForEach(this.ApplyEvent);
        }

        /* Decision Function */
        public void ProvisionCredit(int creditToProvision)
        {
            var creditProvisionedEvent =
                new CreditProvisioned(this.id, this.creditBalance + creditToProvision, creditToProvision);
            eventStore.Save(this.version, creditProvisionedEvent).ForEach(this.ApplyEvent);
        }

        /* Decision Function */
        public void WithdrawCredit(int creditToWithdraw)
        {
            int newCreditBalance = creditBalance - creditToWithdraw;
            if (newCreditBalance < 0)
            {
                throw new Exception("It is not possible to withdraw more money than you have");
            }

            eventStore.Save(version, new CreditWithdrawn(id, newCreditBalance, creditToWithdraw))
                .ForEach(this.ApplyEvent);
        }

        /* Decision Function */
        public string RequestTransfer(string bankAccountDestinationId, int creditToTransfer)
        {
            if (this.id == bankAccountDestinationId)
            {
                throw new Exception("same id");
            }

            if (creditToTransfer > this.creditBalance)
            {
                throw new Exception("not enough money");
            }

            var transferId = Guid.NewGuid().ToString();
            eventStore.Save(this.version,
                new TransferRequested(this.id, bankAccountDestinationId, transferId,
                    this.creditBalance - creditToTransfer, creditToTransfer)).ForEach(this.ApplyEvent);
            return transferId;
        }

        /* Decision Function */
        public void ReceiveTransfer(String bankAccountOriginId, String transferId, int creditTransferred)
        {
            eventStore.Save(version, new TransferReceived(id,
                    transferId,
                    bankAccountOriginId,
                    creditTransferred,
                    creditBalance + creditTransferred))
                .ForEach(this.ApplyEvent);
        }

        /* Decision Function */
        public void CompleteTransfer(string transferId)
        {
            TransferRequested transferRequested = pendingTransfers[transferId];
            if (transferRequested == null)
            {
                throw new Exception(
                    $"transfer designed by id {transferId} has not been requested or was already completed");
            }

            eventStore.Save(version, new TransferCompleted(id,
                    transferId,
                    transferRequested.BankAccountDestinationId))
                .ForEach(this.ApplyEvent);
        }

        public void CancelTransfer(string transferId)
        {
            if (!pendingTransfers.ContainsKey(transferId))
            {
                throw new Exception(
                    $"transfer designed by id {transferId} has not been requested or was already completed");
            }

            TransferRequested transferRequested = pendingTransfers[transferId];
            eventStore.Save(version, new TransferCanceled(id,
                    transferId,
                    transferRequested.BankAccountDestinationId,
                    transferRequested.CreditTransferred,
                    creditBalance + transferRequested.CreditTransferred))
                .ForEach(this.ApplyEvent);
        }

        private void ApplyEvent(Event evt)
        {
            evt.ApplyOn(eventProcessor);
        }

        private Guid generateTransferId()
        {
            return Guid.NewGuid();
        }

        protected bool Equals(BankAccount other)
        {
            return id == other.id && version == other.version && creditBalance == other.creditBalance &&
                   pendingTransfers.SequenceEqual(other.pendingTransfers);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BankAccount) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, version, creditBalance, pendingTransfers);
        }

        private class InnerEventListener : IEventListener
        {
            private readonly BankAccount bankAccount;

            public InnerEventListener(BankAccount bankAccount)
            {
                this.bankAccount = bankAccount;
            }

            public void On(BankAccountRegistered bankAccountRegistered)
            {
                bankAccount.id = bankAccountRegistered.AggregateId;
                bankAccount.version++;
            }

            public void On(CreditProvisioned creditProvisioned)
            {
                bankAccount.creditBalance = creditProvisioned.NewCreditBalance;
                bankAccount.version++;
            }

            public void On(CreditWithdrawn creditWithdrawn)
            {
                bankAccount.creditBalance = creditWithdrawn.NewCreditBalance;
                bankAccount.version++;
            }

            public void On(TransferRequested transferRequested)
            {
                bankAccount.creditBalance = transferRequested.NewCreditBalance;
                bankAccount.pendingTransfers.Add(transferRequested.TransferId, transferRequested);
                bankAccount.version++;
            }

            public void On(TransferReceived transferReceived)
            {
                bankAccount.creditBalance = transferReceived.NewCreditBalance;
                bankAccount.version++;
            }

            public void On(TransferCompleted transferCompleted)
            {
                bankAccount.pendingTransfers.Remove(transferCompleted.TransferId);
                bankAccount.version++;
            }

            public void On(TransferCanceled transferCanceled)
            {
                bankAccount.pendingTransfers.Remove(transferCanceled.TransferId);
                bankAccount.creditBalance = transferCanceled.NewCreditBalance;
                bankAccount.version++;
            }
        }

        public bool IsNull()
        {
            return this.eventStore == null;
        }
    }
}