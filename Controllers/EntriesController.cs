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
    public class EntriesController : Controller
    {
        private IEntryService _entryService;

        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet("{idAccount}")]
        public IActionResult GetByIdAccount(int idAccount)
        {
            var entries = _entryService.GetByIdAccount(idAccount);
            return Ok(entries);
        }

        [HttpPost("addNew")]
        public IActionResult AddNew([FromBody]Entry entry)
        {
            try
            {
                // save 
                _entryService.AddNew(entry);
                return Ok();
            }
            catch (AppException e)
            {
                // return error message if there was an exception
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{idEntry}")]
        public IActionResult Update(int idEntry, [FromBody]Entry entry)
        {
            entry.Id = idEntry;

            try
            {
                // save 
                _entryService.Update(entry);
                return Ok();
            }
            catch (AppException e)
            {
                // return error message if there was an exception
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{idEntry}")]
        public IActionResult Delete(int idEntry)
        {
            _entryService.Delete(idEntry);
            return Ok();
        }
    }
}
