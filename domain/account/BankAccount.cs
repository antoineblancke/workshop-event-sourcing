using System;
using System.Collections.Generic;
using System.Linq;
using domain.common;

namespace domain.account
{
    public class BankAccount
    {
        public static BankAccount RegisterBankAccount(string bankAccountId, EventStore eventStore)
        {
            BankAccount bankAccount = new BankAccount(eventStore);
            bankAccount.RegisterBankAccount(bankAccountId);
            return bankAccount;
        }

        public static BankAccount LoadBankAccount(string bankAccountId, EventStore eventStore)
        {
            var events = eventStore.Load(bankAccountId);
            return events.Any() ? new BankAccount(bankAccountId, eventStore, events) : BankAccount.Empty;
        }

        private static readonly BankAccount Empty = new BankAccount(null);

        private readonly InnerEventListener eventProcessor;

        private readonly string id;

        private readonly int version;

        private EventStore eventStore;

        private readonly int creditBalance;

        private readonly Dictionary<string, TransferRequested> pendingTransfers;

        private BankAccount(EventStore eventStore) : this(string.Empty, eventStore, new List<Event>())
        {
        }

        private BankAccount(string id, EventStore eventStore, List<Event> events) : this(id, eventStore, 0, 0)
        {
            events.ForEach(this.ApplyEvent);
        }

        public BankAccount(string id, EventStore eventStore, int creditBalance, int aggregateVersion) : this(id,
            eventStore, creditBalance, aggregateVersion, new Dictionary<string, TransferRequested>())
        {
        }

        public BankAccount(string id, EventStore eventStore, int creditBalance, int aggregateVersion,
            Dictionary<string, TransferRequested> pendingTransfers)
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
            throw new NotImplementedException();
            /*
              1. instantiate a BankAccountRegistered event
              2. save the event List<Event> savedEvents = eventStore.save(events)
              3. apply saved events on the bank account savedEvents.foreach(this::applyEvent)
            */
        }

        /* Decision Function */
        public void ProvisionCredit(int creditToProvision)
        {
            throw new NotImplementedException();
            /*
              1. instantiate a CreditProvisioned event
              2. save the event List<Event> savedEvents = eventStore.save(events)
              3. apply saved events on the bank account savedEvents.foreach(this::applyEvent)
            */
        }

        /* Decision Function */
        public void WithdrawCredit(int creditToWithdraw)
        {
            throw new NotImplementedException();
            // /!\ creditToWithdraw represent a positive value
            /*
              1. throw an InvalidCommandException if the balance is lower then the credit amount to withdraw
              2. instantiate a CreditWithdrawn event
              3. save the event List<Event> savedEvents = eventStore.save(events)
              4. apply saved events on the bank account savedEvents.foreach(this::applyEvent)
             */
        }

        /* Decision Function */
        public string RequestTransfer(string bankAccountDestinationId, int creditToTransfer)
        {
            throw new NotImplementedException();
            /*
              1. throw an InvalidCommandException if the bank destination id is the same that this id
              2. throw an InvalidCommandException if the balance is lower then the credit amount to transfer
              3. instantiate a TransferRequest event (you can generate a random transfer id by calling UUID.randomUUID)
              4. save the event List<Event> savedEvents = eventStore.save(events)
              5. apply saved events on the bank account savedEvents.foreach(this::applyEvent)
              6. return the transfer id associated the the transfer
            */
        }

        /* Decision Function */
        public void CompleteTransfer(string transferId)
        {
            throw new NotImplementedException();
            /*
              1. throw an InvalidCommandException if the transfer id is absent from the pending transfers map
              2. instantiate a TransferCompleted event
              3. save the event List<Event> savedEvents = EventStore.save(events)
              4. apply saved events on the bank account savedEvents.foreach(this::applyEvent)
         */
        }

        public void CancelTransfer(string transferId)
        {
            throw new NotImplementedException();
            /*
              1. throw an InvalidCommandException if the transfer id is absent from the pending transfers map
              2. instantiate a TransferCanceled event
              3. save the event List<Event> savedEvents = EventStore.save(events)
              4. apply saved events on the bank account savedEvents.foreach(this::applyEvent)
             */
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
            private BankAccount bankAccount;

            public InnerEventListener(BankAccount bankAccount)
            {
                this.bankAccount = bankAccount;
            }

            public void On(BankAccountRegistered bankAccountRegistered)
            {
                throw new NotImplementedException();
                /*
                  1. affect the event's aggregate id to the bank account's id
                  2. increment the aggregate's version
                 */
            }

            public void On(CreditProvisioned creditProvisioned)
            {
                throw new NotImplementedException();
                /*
                  1. affect the event's new credit balance to the bank account's balance
                  2. increment the aggregate's version
                 */
            }

            public void On(CreditWithdrawn creditWithdrawn)
            {
                throw new NotImplementedException();
                /*
                  1. affect the event's new credit balance to the bank account's balance
                  2. increment the aggregate's version
                 */
            }

            public void On(TransferRequested transferRequested)
            {
                throw new NotImplementedException();
                /*
                  1. affect the event's new credit balance to the bank account's balance
                  2. add the event to the pending transfers map
                  3. increment the aggregate's version
                 */
            }

            public void On(TransferReceived transferReceived)
            {
                throw new NotImplementedException();
                /*
                  1. affect the event's new credit balance to the bank account's balance
                  2. increment the aggregate's version
                 */
            }

            public void On(TransferCompleted transferCompleted)
            {
                throw new NotImplementedException();
                /*
                  1. remove the event from the pending transfers map
                  2. increment the aggregate's version
                 */
            }

            public void On(TransferCanceled transferCanceled)
            {
                throw new NotImplementedException();
                /*
                  1. affect the event's new credit balance to the bank account's balance
                  2. remove the event from the pending transfers map
                  3. increment the aggregate's version
                 */
            }
        }
    }
}