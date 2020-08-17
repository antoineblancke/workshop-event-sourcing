using System;
using domain.account;
using NFluent;
using static domain.account.BankAccount;
using Xunit;

namespace domain.acceptance_tests
{
    public class BankAccount_TransferFinalized_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void fail_completing_transfer_never_requested()
        {
            // Given
            BankAccount bankAccount = RegisterBankAccount("bankAccountId", eventStore);

            // When
            Check.ThatCode(() => bankAccount.CompleteTransfer("transferId")).Throws<Exception>();

            // Then
            var events = eventStore.Load("bankAccountId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountId"));
        }

        [Fact]
        public void complete_pending_transaction_with_success()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);
            bankAccountOrigin.ProvisionCredit(1);

            String transferId = bankAccountOrigin.RequestTransfer("bankAccountDestinationId", 1);

            // When
            bankAccountOrigin.CompleteTransfer(transferId);

            // Then
            var events = eventStore.Load("bankAccountOriginId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountOriginId"),
                new TransferRequested("bankAccountOriginId", 
                    "bankAccountDestinationId",
                    transferId,
                    0,
                    1),
                new TransferCompleted("bankAccountOriginId",
                    transferId,
                    "bankAccountDestinationId"));

            Check.That(bankAccountOrigin).IsEqualTo(new BankAccount("bankAccountOriginId", eventStore, 0, 4));
        }

        [Fact]
        public void fail_completing_already_finalized_transaction()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);
            bankAccountOrigin.ProvisionCredit(1);

            String transferId = bankAccountOrigin.RequestTransfer("bankAccountDestinationId", 1);

            bankAccountOrigin.CompleteTransfer(transferId);

            // When
            Check.ThatCode(() => bankAccountOrigin.CompleteTransfer(transferId)).Throws<Exception>();
        }
    }
}