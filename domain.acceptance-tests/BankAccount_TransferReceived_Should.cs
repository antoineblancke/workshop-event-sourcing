using domain.account;
using static domain.account.BankAccount;
using NFluent;
using Xunit;

namespace domain.acceptance_tests
{
    public class BankAccount_TransferReceived_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void receive_transfer() {
            // Given
            BankAccount bankAccountDestination = RegisterBankAccount("bankAccountDestinationId", eventStore);

            // When
            bankAccountDestination.ReceiveTransfer("bankAccountOriginId", "transferId", 1);

            // Then
            var events = eventStore.Load("bankAccountDestinationId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountDestinationId"),
                new TransferReceived("bankAccountDestinationId",
                    "transferId",
                    "bankAccountOriginId",
                    1,
                    1));
            Check.That(bankAccountDestination).IsEqualTo(new BankAccount("bankAccountDestinationId", eventStore, 1, 2));
        }
    }
}