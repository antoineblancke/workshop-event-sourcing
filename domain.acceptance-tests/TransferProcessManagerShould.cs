using System;
using System.Threading;
using domain.account;
using NFluent;
using static domain.account.BankAccount;
using Xunit;

namespace domain.acceptance_tests
{
    public class TransferProcessManagerShould : AbstractBankAccountTesting
    {
        [Fact]
        public void cancel_transfer_when_destination_does_not_exist()
        {
            // Given
            var transferProcessManager = new TransferProcessManager(this.eventStore);
            this.eventBus.Register(transferProcessManager);
            var bankAccount = RegisterBankAccount("bankAccountId", eventStore);
            bankAccount.ProvisionCredit(1);

            // When
            var transferId = bankAccount.RequestTransfer("bankAccountDestinationId", 1);

            // Then
            Thread.Sleep(500);
            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"),
                new CreditProvisioned("bankAccountId", 1, 1),
                new TransferRequested("bankAccountId",
                    "bankAccountDestinationId",
                    transferId,
                    0,
                    1),
                new TransferCanceled("bankAccountId",
                    transferId,
                    "bankAccountDestinationId",
                    1,
                    1));
        }

        [Fact]
        public void complete_transfer_when_destination_exist()
        {
            // Given
            var transferProcessManager = new TransferProcessManager(this.eventStore);
            this.eventBus.Register(transferProcessManager);
            var bankAccount = RegisterBankAccount("bankAccountId", eventStore);
            bankAccount.ProvisionCredit(1);
            var destinationBankAccount = RegisterBankAccount("bankAccountDestinationId", eventStore);

            // When
            var transferId = bankAccount.RequestTransfer("bankAccountDestinationId", 1);

            // Then
            Thread.Sleep(500);
            
            var originEvents = eventStore.Load("bankAccountId");
            Check.That(originEvents).ContainsExactly(new BankAccountRegistered("bankAccountId"),
                new CreditProvisioned("bankAccountId", 1, 1),
                new TransferRequested("bankAccountId", "bankAccountDestinationId", transferId, 0, 1),
                new TransferCompleted("bankAccountId", transferId, "bankAccountDestinationId"));
            
            var destinationEvents = eventStore.Load("bankAccountDestinationId");
            Check.That(destinationEvents).ContainsExactly(new BankAccountRegistered("bankAccountDestinationId"),
                new TransferReceived("bankAccountDestinationId", transferId, "bankAccountId", 1, 1));
        }
    }
}