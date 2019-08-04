using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RecordsController : Controller
    {
        private IRecordService _recordService;
        private IAccountService _accountService;
        private IEntryTypeService _entryTypeService;
        private ICategoryService _categoryService;

        public RecordsController(IRecordService recordService, IAccountService accountService, IEntryTypeService entryTypeService, ICategoryService categoryService)
        {
            _recordService = recordService;
            _accountService = accountService;
            _entryTypeService = entryTypeService;
            _categoryService = categoryService;
        }

        [HttpGet("{idAccount}")]
        public IActionResult GetByIdAccount(int idAccount)
        {
            var entries = _recordService.GetByIdAccount(idAccount);
            return Ok(entries);
        }

        [HttpPost("addNew")]
        public IActionResult AddNew([FromBody]Record record)
        {
            try
            {
                // save 
                _recordService.AddNew(record);

                int entryTypeId = _categoryService.FindCategoryNameByCategiryId(record.CategoryId);
                bool isIncome = _entryTypeService.CheckIfIncome(entryTypeId);
                _accountService.ChangeAccountMoneyAmount(record.AccountId, record.MoneyAmount, isIncome);
                return Ok();
            }
            catch (AppException e)
            {
                // return error message if there was an exception
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{idEntry}")]
        public IActionResult Update(int idEntry, [FromBody]Record record)
        {
            record.Id = idEntry;

            try
            {
                // save 
                _recordService.Update(record);
                return Ok();
            }
            catch (AppException e)
            {
                // return error message if there was an exception
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{idRecord}")]
        public IActionResult Delete(int idRecord)
        {
            _recordService.Delete(idRecord);
            return Ok();
        }
    }
}
