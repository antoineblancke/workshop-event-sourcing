using domain.account;

namespace projection
{
    public class BalanceProjectionManager : IProjectionManager
    {
        private readonly IBalanceRepository balanceRepository;

        public BalanceProjectionManager(IBalanceRepository balanceRepository)
        {
            this.balanceRepository = balanceRepository;
        }

        public void On(BankAccountRegistered bankAccountRegistered)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(CreditProvisioned creditProvisioned)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(CreditWithdrawn creditWithdrawn)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(TransferRequested transferRequested)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(TransferReceived transferReceived)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(TransferCompleted transferCompleted)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(TransferCanceled transferCanceled)
        {
            /*
              project the event by using the balance repository
             */
        }
    }
}