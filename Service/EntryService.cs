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
        void Update(Entry entry);
        void Delete(int idEntry);
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
            return _context.Entries.Where(x => x.AccountId == idAccount).ToList();
        }

        public void AddNew(Entry entry)
        {
            entry.CreatedTimestamp = DateTime.Now.ToString();

            _context.Entries.Add(entry);
            _context.SaveChanges();
        }

        public void Update(Entry entry)
        {
            var newEntry = _context.Entries.Find(entry.Id);

            if (newEntry == null)
                throw new AppException("Entry not found");

            newEntry.MoneyAmount = entry.MoneyAmount;
            newEntry.Description = entry.Description;

            _context.Entries.Update(newEntry);
            _context.SaveChanges();
        }

        public void Delete(int idEntry)
        {
            var entry = _context.Entries.Find(idEntry);
            if (entry != null)
            {
                _context.Entries.Remove(entry);
                _context.SaveChanges();
            }
        }
    }
}
