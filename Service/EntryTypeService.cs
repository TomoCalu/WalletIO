using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalletIO.Entities;
using WalletIO.Helpers;

namespace WalletIO.Service
{
    public interface IEntryTypeService
    {
        IEnumerable<EntryType> GetWithCategories ();
    }

    public class EntryTypeService : IEntryTypeService
    {
        private DataContext _context;

        public EntryTypeService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<EntryType> GetWithCategories (){
            return _context.EntryTypes.Include(x => x.Categories);
        }
    }

}
