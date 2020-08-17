using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using domain.account;
using NFluent;
using static domain.account.BankAccount;
using Xunit;

namespace domain.acceptance_tests
{
    public class BankAccount_TransferRequested_Should : AbstractBankAccountTesting
    {
        [Fact]
        public void fail_requesting_transfer_when_destination_is_same_than_origin()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);

            // When
            Check.ThatCode(() => bankAccountOrigin.RequestTransfer("bankAccountOriginId", 1)).Throws<Exception>();

            // Then
            var events = eventStore.Load("bankAccountOriginId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountOriginId"));
        }

        [Fact]
        public void fail_requesting_transfer_when_credit_to_transfer_greater_than_available_credit()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);

            // When
            Check.ThatCode(() => bankAccountOrigin.RequestTransfer(
                "bankAccountDestinationId",
                1)).Throws<Exception>();

            // Then
            var events = eventStore.Load("bankAccountOriginId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountOriginId"));
        }

        [Fact]
        public void request_transfer()
        {
            // Given
            BankAccount bankAccountOrigin = RegisterBankAccount("bankAccountOriginId", eventStore);
            bankAccountOrigin.ProvisionCredit(1);

            // When
            var transferId = bankAccountOrigin.RequestTransfer("bankAccountDestinationId", 1);

            // Then
            Check.That(transferId).IsNotNull();

            var transferRequested = new TransferRequested("bankAccountOriginId",
                "bankAccountDestinationId",
                transferId,
                0,
                1);

            var events = eventStore.Load("bankAccountOriginId");
            Check.That(events).ContainsExactly(new BankAccountRegistered("bankAccountOriginId"),
                new CreditProvisioned("bankAccountOriginId", 1, 1),
                transferRequested);

            var pendingTransfers = new Dictionary<string, TransferRequested>()
            {
                {transferId, transferRequested}
            };
            Check.That(bankAccountOrigin).IsEqualTo(new BankAccount("bankAccountOriginId",
                eventStore,
                0,
                3,
                new ReadOnlyDictionary<string, TransferRequested>(pendingTransfers)));
        }
    }
}