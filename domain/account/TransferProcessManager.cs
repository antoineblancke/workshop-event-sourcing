using System;
using domain.common;

namespace domain.account
{
    public class TransferProcessManager : IProcessManager
    {
        private readonly EventStore eventStore;


        public TransferProcessManager(EventStore eventStore)
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
            throw new NotImplementedException();
            /*
              1. load the transfer destination bank account (BankAccount.loadBankAccount(...))
              2. if the account exists, send a command to have it receive the transfer
              3. if it does not exist, load the transfer origin account and send a command to cancel the transfer
              4. if the origin account does not exist, or if any exception is thrown by any command, log an error
             */
        }

        public void On(TransferReceived transferReceived)
        {
            throw new NotImplementedException();
            /*
              1. load the transfer origin bank account
              2. if the account exists, send a command to complete the transfer
              3. if the account does not exist, or if any exception is thrown by any command, log an error
             */
        }

        public void On(TransferCompleted transferCompleted)
        {
            throw new NotImplementedException();
            /*
              TransferCompleted event is ignored by the transfer process manager
             */
        }

        public void On(TransferCanceled transferCanceled)
        {
            throw new NotImplementedException();
            /*
              TransferCanceled event is ignored by the transfer process manager
             */
        }
    }
}