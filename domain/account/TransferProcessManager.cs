using System;
using domain.common;
using static domain.account.BankAccount;

namespace domain.account
{
    public class TransferProcessManager : IProcessManager
    {
        private readonly IEventStore eventStore;

        public TransferProcessManager(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }
        public void On(BankAccountRegistered bankAccountRegistered)
        {
        }

        public void On(CreditProvisioned creditProvisioned)
        {
        }

        public void On(CreditWithdrawn creditWithdrawn)
        {
        }

        public void On(TransferRequested transferRequested)
        {
            var destinationBankAccount = LoadBankAccount(transferRequested.BankAccountDestinationId, this.eventStore);

            if (!destinationBankAccount.IsNull())
            {
                destinationBankAccount.ReceiveTransfer(transferRequested.AggregateId, transferRequested.TransferId, transferRequested.CreditTransferred);
                return;
            }
            
            var originBankAccount = LoadBankAccount(transferRequested.AggregateId, this.eventStore);

            if (originBankAccount.IsNull())
            {
                throw new Exception("origin account does not exist");
            }
            
            originBankAccount.CancelTransfer(transferRequested.TransferId);
        }

        public void On(TransferReceived transferReceived)
        {
            var originBankAccount = LoadBankAccount(transferReceived.BankAccountOriginId, eventStore);

            if (originBankAccount.IsNull())
            {
                throw new Exception("origin account does not exist");
            }
            
            originBankAccount.CompleteTransfer(transferReceived.TransferId);
        }

        public void On(TransferCompleted transferCompleted)
        {
            /*
              TransferCompleted event is ignored by the transfer process manager
             */
        }

        public void On(TransferCanceled transferCanceled)
        {
            /*
              TransferCanceled event is ignored by the transfer process manager
             */
        }
    }
}