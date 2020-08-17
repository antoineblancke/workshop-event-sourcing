using System;
using NFluent;
using Xunit;

namespace projection.tests
{
    public class InMemoryBalanceRepositoryShould : IDisposable
    {
        private readonly IBalanceRepository balanceRepository;

        public InMemoryBalanceRepositoryShould()
        {
            this.balanceRepository = new InMemoryBalanceRepository();
        }
        
        public void Dispose()
        {
            this.balanceRepository.Clear();
        }
        
        [Fact]
        public void should_write_credit() {
            // When
            balanceRepository.WriteCreditBalance("bankAccountId", 10);

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(10);
        }

        [Fact]
        public void should_update_credit() {
            // When
            balanceRepository.WriteCreditBalance("bankAccountId", 10);

            // When
            balanceRepository.WriteCreditBalance("bankAccountId", 15);

            // Then
            Check.That(balanceRepository.ReadBalance("bankAccountId")).IsEqualTo(15);
        }
    }
}