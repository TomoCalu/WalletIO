using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletIO.Entities;

namespace WalletIO.Helpers
{
    public class DataSeeder
    {
        private readonly DataContext _context;
        public DataSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedDb() {
            _context.Database.EnsureCreated();
            AddNewEntryType(new EntryType { Name = "Income",
                                            Categories = new List<Category>() { new Category { Name = "Paycheck"},
                                                                                new Category { Name = "Gifts"}}
            });
            AddNewEntryType(new EntryType { Name = "Spendings",
                                            Categories = new List<Category>() { new Category { Name = "Car"},
                                                                                new Category { Name = "Food"}}
            });

            _context.SaveChanges();
        }

        private void AddNewEntryType(EntryType entryType)
        {
            var existingType = _context.EntryTypes.FirstOrDefault(p => p.Name == entryType.Name);
            if (existingType == null)
            {
                _context.EntryTypes.Add(entryType);
            }
        }
    }
}
