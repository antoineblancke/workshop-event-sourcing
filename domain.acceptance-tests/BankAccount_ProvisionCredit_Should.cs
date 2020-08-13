using domain.account;
using NFluent;
using Xunit;
using static domain.account.BankAccount;
namespace domain.acceptance_tests
{
    public class BankAccount_ProvisionCredit_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void provision_credit_with_success() {
            // Given
            BankAccount bankAccount = RegisterBankAccount("bankAccountId", eventStore);

            // When
            bankAccount.ProvisionCredit(1);

            // Then
            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"),
                new CreditProvisioned("bankAccountId", 1, 1));
            Check.That(bankAccount).IsEqualTo(new BankAccount("bankAccountId", eventStore, 1, 2));
        }
    }
}