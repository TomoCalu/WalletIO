using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalletIO.Entities;
using WalletIO.Helpers;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletIO.Service
{
    public interface IRecordService
    {
        IEnumerable<Record> Get();
        Record GetById(int idRecord);
        IEnumerable<Record> GetByIdAccount(int idAccount);
        void AddNew(Record record);
        void Update(Record record);
        void Delete(int idRecord);
        KeyValuePair<string[], decimal[]> GetRecordsDataSum(IEnumerable<EntryType> entryTypesWithCategories, int idUser);
        KeyValuePair<string[], decimal[]> GetBalanceIncomeChangeForThisWeek(int idUser, EntryType incomeEntryType, IEnumerable<Record> records);
        decimal[] GetBalanceSpendingsChangeForThisWeek(int idUser, EntryType incomeEntryType, IEnumerable<Record> records);
    }

    public class RecordService : IRecordService
    {
        private DataContext _context;

        public RecordService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Record> Get()
        {
            return _context.Records.ToList();
        }

        public Record GetById(int idRecord)
        {
            return _context.Records.Find(idRecord); ;
        }

        public IEnumerable<Record> GetByIdAccount(int idAccount)
        {
            return _context.Records.Where(x => x.AccountId == idAccount).ToList();
        }

        public void AddNew(Record record)
        {
            record.CreatedTimestamp = DateTime.Now.ToString();

            _context.Records.Add(record);
            _context.SaveChanges();
        }

        public void Update(Record record)
        {
            var newRecord = _context.Records.Find(record.Id);

            if (newRecord == null)
                throw new AppException("Record not found");

            newRecord.MoneyAmount = record.MoneyAmount;
            newRecord.Description = record.Description;
            newRecord.AccountId = record.AccountId;
            newRecord.CategoryId = record.CategoryId;

            _context.Records.Update(newRecord);
            _context.SaveChanges();
        }

        public void Delete(int idRecord)
        {
            var record = _context.Records.Find(idRecord);
            if (record != null)
            {
                _context.Records.Remove(record);
                _context.SaveChanges();
            }
        }

        public KeyValuePair<string[], decimal[]> GetRecordsDataSum(IEnumerable<EntryType> entryTypesWithCategories, int idUser)
        {

            IEnumerable<EntryType> entryTypesWithCategoriesNoIncome = entryTypesWithCategories.Where(x => x.Name != "Income");
            var arraySize = entryTypesWithCategoriesNoIncome.Count();
            string[] dataSumLabels = new string[arraySize];
            decimal[] dataSumData = new decimal[arraySize];
            for (int i = 0; i < arraySize; ++i)
            {
                EntryType entryTypeWithCategories = entryTypesWithCategoriesNoIncome.ElementAt(i);
                dataSumData[i] = _context.Records.Where(x => entryTypeWithCategories.Categories.Any(y => y.Id == x.CategoryId) &&
                                                             x.Account.UserId == idUser)
                                                 .Sum(x => x.MoneyAmount);
                dataSumLabels[i] = entryTypeWithCategories.Name;
            }

            return new KeyValuePair<string[], decimal[]>(dataSumLabels, dataSumData);
        }

        public KeyValuePair<string[], decimal[]> GetBalanceIncomeChangeForThisWeek(int idUser, EntryType incomeEntryType, IEnumerable<Record> records)
        {
            string[] daysOfThisWeek = new string[7];
            decimal[] incomeForThisWeek = new decimal[7];

            DateTime date = DateTime.Now.Date;
            for (int i = 6; i >= 0; i--)
            {
                incomeForThisWeek[i] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date == date) &&
                                                                    x.Category.EntryTypeId == incomeEntryType.Id &&
                                                                    x.Account.UserId == idUser)
                                                        .Sum(x => x.MoneyAmount);
                daysOfThisWeek[i] = date.ToString("MM/dd/yyyy");
                date = date.AddDays(-1);
            }

            return new KeyValuePair<string[], decimal[]>(daysOfThisWeek, incomeForThisWeek);
        }

        public decimal[] GetBalanceSpendingsChangeForThisWeek(int idUser, EntryType incomeEntryType, IEnumerable<Record> records)
        {
            decimal[] spendingsForThisWeek = new decimal[7];

            DateTime date = DateTime.Now.Date;
            for (int i = 6; i >= 0; i--)
            {
                spendingsForThisWeek[i] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date == date) &&
                                                                       x.Category.EntryTypeId != incomeEntryType.Id &&
                                                                       x.Account.UserId == idUser)
                                                          .Sum(x => x.MoneyAmount);
                date = date.AddDays(-1);
            }

            return spendingsForThisWeek;
        }
    }
}
