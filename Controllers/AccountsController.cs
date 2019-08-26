using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletIO.Entities;
using WalletIO.Helpers;
using WalletIO.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletIO.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{idUser}")]
        public IActionResult GetByIdUser(int idUser)
        {
            var accounts = _accountService.GetByIdUser(idUser);
            return Ok(accounts);
        }

        [HttpPost("addNew")]
        public IActionResult AddNew([FromBody]Account account)
        {
            try
            {
                if(_accountService.CountAccounts(account.UserId) >= 8)
                    return BadRequest(new { message = "Maximum number of accounts reached" });
                // save
                _accountService.AddNew(account);
                return Ok();
            }
            catch (AppException e)
            {
                // return error message if there was an exception
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{idAccount}")]
        public IActionResult Update(int idAccount, [FromBody]Account account)
        {
            account.Id = idAccount;

            try
            {
                // save 
                _accountService.Update(account);
                return Ok();
            }
            catch (AppException e)
            {
                // return error message if there was an exception
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{idAccount}")]
        public IActionResult Delete(int idAccount)
        {
            _accountService.Delete(idAccount);
            return Ok();
        }


    }
}
