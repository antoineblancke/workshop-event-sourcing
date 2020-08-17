using System;
using Xunit;

namespace domain.acceptance_tests
{
    public class TransferProcessManagerShould : AbstractBankAccountTesting
    {
        [Fact]
        public void cancel_transfer_when_destination_does_not_exist()
        {
            throw new NotImplementedException();
            // Given
            /*
              1. a transfer process manager registered with the event bus (the event bus is accessible from the superclass)
              2. a registration of the process manager into the event bus (super.eventBus.register(...))
              3. a bank account ("origin") registered and provisioned with 1 credit
             */

            // When
            /*
              when a transfer is initialized from "origin" to a non registered bank account id
             */

            // Then
            /*
              1. Wait for the event bus to process events
              2. "origin" events should contains exactly 1 BankAccountRegistered, 1 CreditProvisioned, 1 TransferRequested and 1 TransferCanceled
             */
        }

        [Fact]
        public void complete_transfer_when_destination_exist()
        {
            throw new NotImplementedException();
            // Given
            /*
              1. a transfer process manager registered with the event bus (the event bus is accessible from the superclass)
              2. a registration of the process manager into the event bus (super.eventBus.register(...))
              3. a bank account ("origin") registered and provisioned with 1 credit
              4. a bank account ("destination") registered
             */

            // When
            /*
              when a transfer is requested from "origin" to "destination"
             */

            // Then
            /*
             1. Wait for the event bus to process events
             2. "origin" events should contain exactly 1 BankAccountRegistered, 1 CreditProvisioned, 1 TransferRequested and 1 TransferCompleted
             3. "destinations" events should contain exactly 1 BankAccountRegistered and 1 TransferReceived
             */
        }
    }
}