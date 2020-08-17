using System;
using domain.account;
using NFluent;
using static domain.account.BankAccount;
using Xunit;

namespace domain.acceptance_tests
{
    public class BankAccount_TransferCanceled_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void should_fail_canceling_transfer_never_requested()
        {
            // Given
            BankAccount bankAccount = RegisterBankAccount("bankAccountId", eventStore);

            // When
            Check.ThatCode(() => bankAccount.CancelTransfer("transferId")).Throws<Exception>();

            // Then
            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"));
        }

        [Fact]
        public void should_cancel_pending_transaction_with_success()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);
            bankAccountOrigin.ProvisionCredit(1);

            String transferId = bankAccountOrigin.RequestTransfer("bankAccountDestinationId", 1);

            // When
            bankAccountOrigin.CancelTransfer(transferId);

            // Then
            var events = eventStore.Load("bankAccountOriginId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountOriginId"),
                new CreditProvisioned("bankAccountOriginId", 1, 1),
                new TransferRequested("bankAccountOriginId", transferId,
                    "bankAccountDestinationId",
                    1,
                    0),
                new TransferCanceled("bankAccountOriginId",
                    transferId,
                    "bankAccountDestinationId",
                    1,
                    1));

            Check.That(bankAccountOrigin).IsEqualTo(new BankAccount("bankAccountOriginId", eventStore, 1, 4));
        }

        [Fact]
        public void should_fail_canceling_already_canceled_transaction()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);
            bankAccountOrigin.ProvisionCredit(1);

            String transferId = bankAccountOrigin.RequestTransfer("bankAccountDestinationId", 1);

            bankAccountOrigin.CancelTransfer(transferId);

            // When
            Check.ThatCode(() => bankAccountOrigin.CancelTransfer(transferId)).Throws<Exception>();

            // Then
            var events = eventStore.Load("bankAccountOriginId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountOriginId"),
                new CreditProvisioned("bankAccountOriginId", 1, 1),
                new TransferRequested("bankAccountOriginId",
                    "bankAccountDestinationId",
                    transferId,
                    0,
                    1),
                new TransferCanceled("bankAccountOriginId",
                    transferId,
                    "bankAccountDestinationId",
                    1,
                    1));
        }
    }
}