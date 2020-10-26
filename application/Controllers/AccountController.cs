using System;
using domain.account;
using static domain.account.BankAccount;
using domain.common;
using Microsoft.AspNetCore.Mvc;
using projection;

namespace application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly IBalanceRepository balanceRepository;
        private readonly IEventStore eventStore;

        public AccountController(IBalanceRepository balanceRepository, IEventStore eventStore)
        {
            this.balanceRepository = balanceRepository;
            this.eventStore = eventStore;
        }

        [HttpGet]
        [Route("{bankAccountId}/balance")]
        public int Get([FromRoute] string bankAccountId)
        {
            return this.balanceRepository.ReadBalance(bankAccountId) ?? 0;
        }

        [HttpPost]
        [Route("{bankAccountId}")]
        public IActionResult Create(string bankAccountId)
        {
            RegisterBankAccount(bankAccountId, eventStore);
            return this.Ok();
        }
        
        [HttpPost]
        [Route("{bankAccountId}/provision/{credit}")]
        public IActionResult ProvisionCredit(string bankAccountId, int credit)
        {
            GetBankAccount(bankAccountId).ProvisionCredit(credit);
            return this.Ok();
        }
        
        [HttpPost]
        [Route("{bankAccountId}/withdraw/{credit}")]
        public IActionResult WthdrawCredit(string bankAccountId, int credit)
        {
            GetBankAccount(bankAccountId).WithdrawCredit(credit);
            return this.Ok();
        }
        
        [HttpPost]
        [Route("{bankAccountId}/transfer/{credit}/to/{bankAccountDestinationId}")]
        public IActionResult TransferCreditTo(string bankAccountId, int credit, string bankAccountDestinationId)
        {
            GetBankAccount(bankAccountId).RequestTransfer(bankAccountDestinationId, credit);
            return this.Ok();
        }
        
        private BankAccount GetBankAccount(String bankAccountId)
        {
            return BankAccount.LoadBankAccount(bankAccountId, eventStore);
        }
    }
}