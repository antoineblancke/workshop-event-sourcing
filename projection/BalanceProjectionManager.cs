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
            this.balanceRepository.WriteCreditBalance(bankAccountRegistered.AggregateId, 0);
        }

        public void On(CreditProvisioned creditProvisioned)
        {
            this.balanceRepository.WriteCreditBalance(creditProvisioned.AggregateId, creditProvisioned.NewCreditBalance);
        }

        public void On(CreditWithdrawn creditWithdrawn)
        {
            this.balanceRepository.WriteCreditBalance(creditWithdrawn.AggregateId, creditWithdrawn.NewCreditBalance);
        }

        public void On(TransferRequested transferRequested)
        {
            this.balanceRepository.WriteCreditBalance(transferRequested.AggregateId, transferRequested.NewCreditBalance);
        }

        public void On(TransferReceived transferReceived)
        {
            this.balanceRepository.WriteCreditBalance(transferReceived.AggregateId, transferReceived.NewCreditBalance);
        }

        public void On(TransferCompleted transferCompleted)
        {
            /*
              project the event by using the balance repository
             */
        }

        public void On(TransferCanceled transferCanceled)
        {
            balanceRepository.WriteCreditBalance(transferCanceled.AggregateId, transferCanceled.NewCreditBalance);
        }
    }
}