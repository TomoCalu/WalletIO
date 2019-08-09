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
        EntryType GetById(int idEntryType);
        IEnumerable<EntryType> GetWithCategories ();
        bool CheckIfIncome(int idEntryType);
        EntryType GetIncomeObject();
    }

    public class EntryTypeService : IEntryTypeService
    {
        private DataContext _context;

        public EntryTypeService(DataContext context)
        {
            _context = context;
        }

        public EntryType GetById(int idEntryType)
        {
            return _context.EntryTypes.Find(idEntryType);
        }

        public IEnumerable<EntryType> GetWithCategories (){

            return _context.EntryTypes.Include(x => x.Categories);
        }

        public bool CheckIfIncome(int idEntryType)
        {
            var entryType = _context.EntryTypes.Find(idEntryType);
            if (entryType.Name == "Income") return true;
            else return false;
        }

        public EntryType GetIncomeObject()
        {
            return _context.EntryTypes.Where(x => x.Name == "Income").FirstOrDefault();
        }
    }

}
