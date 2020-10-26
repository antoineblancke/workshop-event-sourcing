using domain.account;

namespace domain.common
{
    public interface IEventListener
    {
        void On(BankAccountRegistered bankAccountRegistered);

        void On(CreditProvisioned creditProvisioned);

        void On(CreditWithdrawn creditWithdrawn);

        void On(TransferRequested transferRequested);

        void On(TransferReceived transferReceived);

        void On(TransferCompleted transferCompleted);

        void On(TransferCanceled transferCanceled);
    }
}