using domain.common;

namespace domain.account
{
    public class BankAccountRegistered : Event
    {
        public BankAccountRegistered(string bankAccountId) : base(bankAccountId)
        {
        }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }
    }
}