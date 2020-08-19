using System;
using domain.account;
using NFluent;
using Xunit;

namespace projection.tests
{
    public class BalanceProjectionManagerShould : IDisposable
    {
        private readonly BalanceProjectionManager balanceProjectionManager;
        private readonly IBalanceRepository balanceRepository;

        public BalanceProjectionManagerShould()
        {
            this.balanceRepository = new InMemoryBalanceRepository();
            this.balanceProjectionManager = new BalanceProjectionManager(balanceRepository);
        }

        public void Dispose()
        {
            this.balanceRepository.Clear();
        }

        [Fact]
        public void onBankAccountRegistered()
        {
            // When
            balanceProjectionManager.On(new BankAccountRegistered("bankAccountId"));

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(0);
        }

        [Fact]
        public void onCreditProvisioned()
        {
            // When
            balanceProjectionManager.On(new CreditProvisioned("bankAccountId", 15, 10));

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(15);
        }

        [Fact]
        public void onCreditWithdrawn()
        {
            // When
            balanceProjectionManager.On(new CreditWithdrawn("bankAccountId", 15, 10));

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(15);
        }

        [Fact]
        public void onTransferRequested()
        {
            // When
            balanceProjectionManager.On(new TransferRequested("bankAccountId", "transferId",
                "bankAccountDestination",
                15, 10
            ));

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(15);
        }

        [Fact]
        public void onTransferReceived()
        {
            // When
            balanceProjectionManager.On(new TransferReceived("bankAccountId",
                "transferId",
                "bankAccountDestination",
                15, 10
            ));

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(15);
        }

        [Fact]
        public void onTransferCanceled()
        {
            // When
            balanceProjectionManager.On(new TransferCanceled("bankAccountId",
                "transferId",
                "bankAccountDestination",
                15, 10));

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(15);
        }
    }
}