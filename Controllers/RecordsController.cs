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

        [HttpGet("{idUser}")]
        public IActionResult GetByIdUserWithAccountEntryAndCategory(int idUser)
        {
            var accounts = _accountService.GetByIdUser(idUser);
            IEnumerable<Record> records = Enumerable.Empty<Record>();
            foreach (Account account in accounts)
            {
                records = records.Concat(_recordService.GetByIdAccount(account.Id));
            }
            foreach(Record record in records)
            {
                record.Category = _categoryService.GetById(record.CategoryId);
                record.EntryType = _entryTypeService.GetById(record.Category.EntryTypeId);
                record.Account = _accountService.GetById(record.AccountId);
            }
            return Ok(records.OrderByDescending(x => x.CreatedTimestamp));
        }

        [HttpPost("addNew")]
        public IActionResult AddNew([FromBody]Record record)
        {
            try
            {
                // save 
                _recordService.AddNew(record);
                int entryTypeId = _categoryService.GetEntryTypeIdFromCategoryId(record.CategoryId);
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

        [HttpPut("{idRecord}")]
        public IActionResult Update(int idRecord, [FromBody]Record record)
        {
            decimal moneyAmountDifference;
            record.Id = idRecord;

            try
            {
                Record oldRecord = _recordService.GetById(idRecord);
                int oldEntryTypeId = _categoryService.GetEntryTypeIdFromCategoryId(oldRecord.CategoryId);
                bool oldIsIncome = _entryTypeService.CheckIfIncome(oldEntryTypeId);
                int entryTypeId = _categoryService.GetEntryTypeIdFromCategoryId(record.CategoryId);
                bool isIncome = _entryTypeService.CheckIfIncome(entryTypeId);

                if (oldIsIncome != isIncome)
                {
                    moneyAmountDifference = oldRecord.MoneyAmount + record.MoneyAmount;
                }
                else
                {
                    if (isIncome == false) moneyAmountDifference = record.MoneyAmount - oldRecord.MoneyAmount;
                    else moneyAmountDifference = record.MoneyAmount - oldRecord.MoneyAmount;
                }

                _accountService.ChangeAccountMoneyAmount(record.AccountId, moneyAmountDifference, isIncome);
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
            Record record = _recordService.GetById(idRecord);
            int entryTypeId = _categoryService.GetEntryTypeIdFromCategoryId(record.CategoryId);
            bool isIncome = _entryTypeService.CheckIfIncome(entryTypeId);

            record.MoneyAmount = record.MoneyAmount * -1;

            _accountService.ChangeAccountMoneyAmount(record.AccountId, record.MoneyAmount, isIncome);
            _recordService.Delete(idRecord);

            return Ok();
        }

        [HttpGet("recordDataSum/{idUser}")]
        public IActionResult GetRecordDatSum(int idUser)
        {
            var entryTypes = _entryTypeService.GetWithCategories();
            var recordsDataSum = _recordService.GetRecordsDataSum(entryTypes, idUser);
            return Ok(recordsDataSum);
        }

        [HttpGet("incomeAndSpendingTrends/{idUser}/{selectedRange}")]
        public IActionResult GetIncomeTrends(int idUser, string selectedRange)
        {
            /*if (idAccount == null)
            {
                IEnumerable<Account> accounts = Enumerable.Empty<Account>();
                accounts = _accountService.GetByIdUser(idUser);
            }
            else Account accounts = _accountService.GetById(idAccount);*/
            var accounts = _accountService.GetByIdUser(idUser);

            var incomeEntryType = _entryTypeService.GetIncomeObject();
            IEnumerable<Record> records = Enumerable.Empty<Record>();

            foreach (Account account in accounts)
            {
                records = records.Concat(_recordService.GetByIdAccount(account.Id));
            }
            if (selectedRange == "Week")
            {
                var incomeTrendsSum = _recordService.GetBalanceChangeForThisWeek(idUser, incomeEntryType, null);
                return Ok(incomeTrendsSum);
            }

            else if (selectedRange == "Month")
            {
                var incomeTrendsSum = _recordService.GetBalanceChangeForThisMonth(idUser, incomeEntryType, null);
                return Ok(incomeTrendsSum);
            }

            else if (selectedRange == "Year")
            {
                var incomeTrendsSum = _recordService.GetBalanceChangeForThisYear(idUser, incomeEntryType, null);
                return Ok(incomeTrendsSum);
            }

            return BadRequest(new { message = "Wrong time range" });

        }
    }
}
