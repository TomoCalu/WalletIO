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
            AddNewEntryType(new EntryType
            {
                Name = "Food & Drinks",
                Categories = new List<Category>() { new Category { Name = "General - Food & Drinks"},
                                                    new Category { Name = "Bar, cafe"},
                                                    new Category { Name = "Groceries"},
                                                    new Category { Name = "Restaurant, fast-food"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Shopping",
                Categories = new List<Category>() { new Category { Name = "General - Shopping"},
                                                    new Category { Name = "Clothes & shoes"},
                                                    new Category { Name = "Drug-store, chemist"},
                                                    new Category { Name = "Electronics, accesories"},
                                                    new Category { Name = "Free time"},
                                                    new Category { Name = "Gifts, joy"},
                                                    new Category { Name = "Health and beauty"},
                                                    new Category { Name = "Home, garden"},
                                                    new Category { Name = "Jewels, accesories"},
                                                    new Category { Name = "Kids"},
                                                    new Category { Name = "Pets, animals"},
                                                    new Category { Name = "Stationery, tools"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Housing",
                Categories = new List<Category>() { new Category { Name = "General - Housing"},
                                                    new Category { Name = "Energy, utilities"},
                                                    new Category { Name = "Maintenance, repairs"},
                                                    new Category { Name = "Mortgage"},
                                                    new Category { Name = "Property insurance"},
                                                    new Category { Name = "Rent"},
                                                    new Category { Name = "Services"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Transportation",
                Categories = new List<Category>() { new Category { Name = "General - Transportation"},
                                                    new Category { Name = "Business trips"},
                                                    new Category { Name = "Long distance"},
                                                    new Category { Name = "Public transport"},
                                                    new Category { Name = "Taxi"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Vehicle",
                Categories = new List<Category>() { new Category { Name = "General - Vehicle"},
                                                    new Category { Name = "Fuel"},
                                                    new Category { Name = "Leasing"},
                                                    new Category { Name = "Parking"},
                                                    new Category { Name = "Rentals"},
                                                    new Category { Name = "Vehicle insurance"},
                                                    new Category { Name = "Vehicle maintanance"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Life & Entertainment",
                Categories = new List<Category>() { new Category { Name = "General - Life & Entertainment"},
                                                    new Category { Name = "Active sport, fitness"},
                                                    new Category { Name = "Alcohol, tobacco"},
                                                    new Category { Name = "Books, audio, subscriptions"},
                                                    new Category { Name = "Charity, gifts"},
                                                    new Category { Name = "Culture, sport events"},
                                                    new Category { Name = "Education, development"},
                                                    new Category { Name = "Health care, doctor"},
                                                    new Category { Name = "Hobbies"},
                                                    new Category { Name = "Holiday, trips, hotels"},
                                                    new Category { Name = "Life events"},
                                                    new Category { Name = "Lottery, gambling"},
                                                    new Category { Name = "TV, Streaming"},
                                                    new Category { Name = "Welness, beauty"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Communication, PC",
                Categories = new List<Category>() { new Category { Name = "General - Communication, PC"},
                                                    new Category { Name = "Internet"},
                                                    new Category { Name = "Phone, mobile phone"},
                                                    new Category { Name = "Postal services"},
                                                    new Category { Name = "Software, apps, games"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Financial expenses",
                Categories = new List<Category>() { new Category { Name = "General - Financial expenses"},
                                                    new Category { Name = "Advisory"},
                                                    new Category { Name = "Charges, Fees"},
                                                    new Category { Name = "Child Support"},
                                                    new Category { Name = "Fines"},
                                                    new Category { Name = "Insurances"},
                                                    new Category { Name = "Loan, interests"},
                                                    new Category { Name = "Taxes"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Investments",
                Categories = new List<Category>() { new Category { Name = "General - Investments"},
                                                    new Category { Name = "Collections"},
                                                    new Category { Name = "Financial investments"},
                                                    new Category { Name = "Realty"},
                                                    new Category { Name = "Savings"},
                                                    new Category { Name = "Vehicles, chattels"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Income",
                Categories = new List<Category>() { new Category { Name = "General - Income"},
                                                    new Category { Name = "Checks, coupons"},
                                                    new Category { Name = "Child support"},
                                                    new Category { Name = "Dues & grants"},
                                                    new Category { Name = "Gifts"},
                                                    new Category { Name = "Interests, dividend"},
                                                    new Category { Name = "Lending, renting"},
                                                    new Category { Name = "Lottery, gambling"},
                                                    new Category { Name = "Refunds (tax, purchase)"},
                                                    new Category { Name = "Rental income"},
                                                    new Category { Name = "Sale"},
                                                    new Category { Name = "Wage, invoices"} }
            });
            AddNewEntryType(new EntryType
            {
                Name = "Others",
                Categories = new List<Category>() { new Category { Name = "General - Others" } }
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
