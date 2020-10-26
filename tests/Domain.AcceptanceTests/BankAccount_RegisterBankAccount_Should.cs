using System;

using Xunit;

namespace Domain.AcceptanceTests
{
  public class BankAccount_RegisterBankAccount_Should : AbstractBankAccountTesting
  {
    [Fact]
    public void register_bank_account_with_success()
    {
      throw new NotImplementedException();
      // When
      /*
        when a bank account is registered (BankAccount.registerBankAccount)
       */

      // Then
      /*
        1. assert that the events associated to the bank account contains exactly one BankAccountRegistered event (use assertThatEvents method defined in the superclass)
        * assertThatEvents(...).containsExactly(...)
        2. the test fails, implement BankAccount.registerBankAccount decision function
        3. assert on the state of the bank account (you can use Assertion.assertThat(actualBankAccount).isEqualTo(expectedBankAccount)) :
        * it's id should be identical to the one created
        * its credit should be equal to 0
        * its version should be 1 (one event has been applied on the bank account)
        4. the test fails, implement BankAccount.innerEventListener.on(BankAccountRegistered) evolution function
       */
    }

    [Fact]
    public void fail_registering_bank_account_with_already_used_id()
    {
      throw new NotImplementedException();
      // Given
      /*
        Given a bank account registered (BankAccount.registerBankAccount)
       */

      // When
      /*
        When a bank account with the same id is registered (use Assertions.catchThrowable(() -> BankAccount.registerBankAccount(...)) to catch the exception)
        * Throwable throwable = Assertions.catchThrowable(() -> BankAccount.registerBankAccount(...))
       */

      // Then
      /*
        1. assert that the command thrown a ConflictingEventException exception
        * Assertions.assertThat(throwable).isInstanceOf(ConflictingEventException.class)
        2. assert that the events associated to the bank account contains exactly one BankAccountRegistered event
       */
    }
  }
}
