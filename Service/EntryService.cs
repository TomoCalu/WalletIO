using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalletIO.Entities;
using WalletIO.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletIO.Service
{
    public interface IEntryService
    {
        IEnumerable<Entry> GetByIdAccount(int idAccount);
        void AddNew(Entry entry);
    }

    public class EntryService : IEntryService
    {
        private DataContext _context;

        public EntryService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Entry> GetByIdAccount(int idAccount)
        {
            return _context.Entries.Where(x => x.Account.Id == idAccount).ToList();
        }

        public void AddNew(Entry entry)
        {
            entry.CreatedTimestamp = DateTime.Now.ToString();

            _context.Entries.Add(entry);
            _context.SaveChanges();
        }
    }
}
