using System;

using Domain.Account;

using NFluent;
using static Domain.Account.BankAccount;
using Xunit;

namespace Domain.AcceptanceTests
{
    public class BankAccount_WithdrawCredit_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void Fail_Withdrawing_More_Credit_Than_Provisioned()
        {
            // Given
            BankAccount bankAccount = RegisterBankAccount("bankAccountId", eventStore);

            // When
            Check.ThatCode(() => bankAccount.WithdrawCredit(1)).Throws<Exception>();

            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"));
        }

        [Fact]
        public void Succeed_Withdrawing_Less_Credit_Than_Provisioned()
        {
            // Given
            BankAccount bankAccount = RegisterBankAccount("bankAccountId", eventStore);
            bankAccount.ProvisionCredit(1);

            // When
            bankAccount.WithdrawCredit(1);

            // Then
            Check.That(eventStore.Load("bankAccountId")).ContainsExactly(new BankAccountRegistered("bankAccountId"),
                new CreditProvisioned("bankAccountId", 1, 1),
                new CreditWithdrawn("bankAccountId", 1, 0));

            Check.That(bankAccount).IsEqualTo(new BankAccount("bankAccountId", eventStore, 0, 3));
        }
    }
}
