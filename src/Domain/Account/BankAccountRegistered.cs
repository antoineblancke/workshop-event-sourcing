using Domain.Common;

namespace Domain.Account
{
    public class BankAccountRegistered : Event
    {
        public BankAccountRegistered(string bankAccountId) : base(bankAccountId)
        { }

        public override void ApplyOn(IEventListener eventListener)
        {
            eventListener.On(this);
        }
    }
}
