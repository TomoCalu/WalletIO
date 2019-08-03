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
    public class EntryTypesController : Controller
    {
        private IEntryTypeService _entryTypeService;

        public EntryTypesController(IEntryTypeService entryTypeService)
        {
            _entryTypeService = entryTypeService;
        }

        [HttpGet]
        public IActionResult GetWithCategories(int idUser)
        {
            var entryTypesWithCategories = _entryTypeService.GetWithCategories();
            return Ok(entryTypesWithCategories);
        }
    }
}
