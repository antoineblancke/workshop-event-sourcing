using System;
using domain.account;
using NFluent;
using static domain.account.BankAccount;
using Xunit;

namespace domain.acceptance_tests
{
    public class BankAccount_RegisterBankAccount_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void register_bank_account_with_success()
        {
            var bankAccount = RegisterBankAccount("bankAccountId", eventStore);

            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"));
            Check.That(bankAccount).IsEqualTo(new BankAccount("bankAccountId", eventStore, 0, 1));
        }

        [Fact]
        public void throw_registering_bank_account_with_already_used_id()
        {
            var bankAccount = RegisterBankAccount("bankAccountId", eventStore);

            Check.ThatCode(() => RegisterBankAccount("bankAccountId", eventStore)).Throws<Exception>();

            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"));
        }
    }
}