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
        KeyValuePair<string[], decimal[]> GetRecordsDataSum(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories);
        KeyValuePair<string[], decimal[]> GetRecordsDataSumForLastWeek(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories);
        KeyValuePair<string[], decimal[]> GetRecordsDataSumForLastMonth(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories);
        KeyValuePair<string[], decimal[]> GetRecordsDataSumForLastYear(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories);
        Tuple<string[], decimal[], decimal[]> GetBalanceChangeForThisWeek(int?[] selectedAccounts, EntryType incomeEntryType);
        Tuple<string[], decimal[], decimal[]> GetBalanceChangeForThisMonth(int?[] selectedAccounts, EntryType incomeEntryType);
        Tuple<string[], decimal[], decimal[]> GetBalanceChangeForThisYear(int?[] selectedAccounts, EntryType incomeEntryType);
        KeyValuePair<int?[], decimal[]> GetSpendingAndIncomePerCategory(int?[] selectedCategories, IEnumerable<Record> records);
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

        public KeyValuePair<string[], decimal[]> GetRecordsDataSum(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories)
        {

            IEnumerable<EntryType> entryTypesWithCategoriesNoIncome = entryTypesWithCategories.Where(x => x.Name != "Income");
            var arraySize = entryTypesWithCategoriesNoIncome.Count();
            string[] dataSumLabels = new string[arraySize];
            decimal[] dataSumData = new decimal[arraySize];
            for (int i = 0; i < arraySize; ++i)
            {
                EntryType entryTypeWithCategories = entryTypesWithCategoriesNoIncome.ElementAt(i);
                dataSumData[i] = _context.Records.Where(x => entryTypeWithCategories.Categories.Any(y => y.Id == x.CategoryId) &&
                                                             selectedAccounts.Contains(x.AccountId))
                                                 .Sum(x => x.MoneyAmount);
                dataSumLabels[i] = entryTypeWithCategories.Name;
            }

            return new KeyValuePair<string[], decimal[]>(dataSumLabels, dataSumData);
        }

        public KeyValuePair<string[], decimal[]> GetRecordsDataSumForLastWeek(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories)
        {
            IEnumerable<EntryType> entryTypesWithCategoriesNoIncome = entryTypesWithCategories.Where(x => x.Name != "Income");
            var arraySize = entryTypesWithCategoriesNoIncome.Count();
            string[] dataSumLabels = new string[arraySize];
            decimal[] dataSumData = new decimal[arraySize];
            DateTime date = DateTime.Now.Date;

            for (int i = 0; i < arraySize; ++i)
            {
                EntryType entryTypeWithCategories = entryTypesWithCategoriesNoIncome.ElementAt(i);
                dataSumData[i] = _context.Records.Where(x => entryTypeWithCategories.Categories.Any(y => y.Id == x.CategoryId) &&
                                                             selectedAccounts.Contains(x.AccountId) &&
                                                             DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date <= date &&
                                                             DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date > date.AddDays(-7))
                                                 .Sum(x => x.MoneyAmount);
                dataSumLabels[i] = entryTypeWithCategories.Name;
            }

            return new KeyValuePair<string[], decimal[]>(dataSumLabels, dataSumData);
        }

        public KeyValuePair<string[], decimal[]> GetRecordsDataSumForLastMonth(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories)
        {
            IEnumerable<EntryType> entryTypesWithCategoriesNoIncome = entryTypesWithCategories.Where(x => x.Name != "Income");
            var arraySize = entryTypesWithCategoriesNoIncome.Count();
            string[] dataSumLabels = new string[arraySize];
            decimal[] dataSumData = new decimal[arraySize];
            DateTime date = DateTime.Now.Date;

            for (int i = 0; i < arraySize; ++i)
            {
                EntryType entryTypeWithCategories = entryTypesWithCategoriesNoIncome.ElementAt(i);
                dataSumData[i] = _context.Records.Where(x => entryTypeWithCategories.Categories.Any(y => y.Id == x.CategoryId) &&
                                                             selectedAccounts.Contains(x.AccountId) &&
                                                             DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date <= date &&
                                                             DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date > date.AddDays(-30))
                                                 .Sum(x => x.MoneyAmount);
                dataSumLabels[i] = entryTypeWithCategories.Name;
            }

            return new KeyValuePair<string[], decimal[]>(dataSumLabels, dataSumData);
        }

        public KeyValuePair<string[], decimal[]> GetRecordsDataSumForLastYear(int?[] selectedAccounts, IEnumerable<EntryType> entryTypesWithCategories)
        {
            IEnumerable<EntryType> entryTypesWithCategoriesNoIncome = entryTypesWithCategories.Where(x => x.Name != "Income");
            var arraySize = entryTypesWithCategoriesNoIncome.Count();
            string[] dataSumLabels = new string[arraySize];
            decimal[] dataSumData = new decimal[arraySize];
            DateTime date = DateTime.Now.Date;

            for (int i = 0; i < arraySize; ++i)
            {
                EntryType entryTypeWithCategories = entryTypesWithCategoriesNoIncome.ElementAt(i);
                dataSumData[i] = _context.Records.Where(x => entryTypeWithCategories.Categories.Any(y => y.Id == x.CategoryId) &&
                                                             selectedAccounts.Contains(x.AccountId) &&
                                                             DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date <= date &&
                                                             DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date > date.AddDays(-365))
                                                 .Sum(x => x.MoneyAmount);
                dataSumLabels[i] = entryTypeWithCategories.Name;
            }

            return new KeyValuePair<string[], decimal[]>(dataSumLabels, dataSumData);
        }

        public Tuple<string[], decimal[], decimal[]> GetBalanceChangeForThisWeek(int?[] selectedAccounts, EntryType incomeEntryType)
        {
            string[] daysOfThisWeek = new string[7];
            decimal[] incomeForThisWeek = new decimal[7];
            decimal[] spendingsForThisWeek = new decimal[7];

            DateTime date = DateTime.Now.Date;
            for (int i = 6; i >= 0; i--)
            {
                incomeForThisWeek[i] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date == date) &&
                                                                    x.Category.EntryTypeId == incomeEntryType.Id &&
                                                                    selectedAccounts.Contains(x.AccountId))
                                                        .Sum(x => x.MoneyAmount);

                spendingsForThisWeek[i] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date == date) &&
                                                                       x.Category.EntryTypeId != incomeEntryType.Id &&
                                                                       selectedAccounts.Contains(x.AccountId))
                                                          .Sum(x => x.MoneyAmount);
                daysOfThisWeek[i] = date.ToString("MM/dd/yyyy");
                date = date.AddDays(-1);
            }

            return Tuple.Create(daysOfThisWeek, incomeForThisWeek, spendingsForThisWeek);
        }

        public Tuple<string[], decimal[], decimal[]> GetBalanceChangeForThisMonth(int?[] selectedAccounts, EntryType incomeEntryType)
        {
            string[] weeksOfThisMonth = new string[6];
            decimal[] incomeForThisMonth = new decimal[6];
            decimal[] spendingsForThisMonth = new decimal[6];

            DateTime date = DateTime.Now.Date;
            for (int i = 35; i >= 0; i=i-7)
            {
                incomeForThisMonth[i/7] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date <= date) &&
                                                                   (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date > date.AddDays(-7)) &&
                                                                   x.Category.EntryTypeId == incomeEntryType.Id &&
                                                                   selectedAccounts.Contains(x.AccountId))
                                                        .Sum(x => x.MoneyAmount);

                spendingsForThisMonth[i / 7] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date <= date) &&
                                                                     (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Date > date.AddDays(-7)) &&
                                                                     x.Category.EntryTypeId != incomeEntryType.Id &&
                                                                     selectedAccounts.Contains(x.AccountId))
                                                        .Sum(x => x.MoneyAmount);

                weeksOfThisMonth[i/7] = date.ToString("MM/dd/yyyy");
                date = date.AddDays(-7);
            }

            return Tuple.Create(weeksOfThisMonth, incomeForThisMonth, spendingsForThisMonth);
        }
        public Tuple<string[], decimal[], decimal[]> GetBalanceChangeForThisYear(int?[] selectedAccounts, EntryType incomeEntryType)
        {
            string[] monthsOfThisYear = new string[12];
            decimal[] incomeForThisYear = new decimal[12];
            decimal[] spendingsForThisYear = new decimal[12];

            DateTime currentDate = DateTime.Now.Date;
            string month = currentDate.ToString("MMMM yyyy");
            for (int i = 11; i >= 0; i--)
            {
                incomeForThisYear[i] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Month == currentDate.Month) &&
                                                                   (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Year == currentDate.Year) &&
                                                                   x.Category.EntryTypeId == incomeEntryType.Id &&
                                                                   selectedAccounts.Contains(x.AccountId))
                                                       .Sum(x => x.MoneyAmount);

                spendingsForThisYear[i] = _context.Records.Where(x => (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Month == currentDate.Month) &&
                                                                      (DateTime.ParseExact(x.CreatedTimestamp, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture).Year == currentDate.Year) &&
                                                                      x.Category.EntryTypeId != incomeEntryType.Id &&
                                                                      selectedAccounts.Contains(x.AccountId))
                                                          .Sum(x => x.MoneyAmount);

                monthsOfThisYear[i] = month;
                currentDate = currentDate.AddMonths(-1);
                month = currentDate.ToString("MMMM yyyy");
            }

            return Tuple.Create(monthsOfThisYear, incomeForThisYear, spendingsForThisYear);
        }

        public KeyValuePair<int?[], decimal[]> GetSpendingAndIncomePerCategory(int?[] selectedCategories, IEnumerable<Record> records)
        {
            decimal[] moneyAmount = new decimal[selectedCategories.Length];
            for (int i=0; i < selectedCategories.Length; i++)
            {
                moneyAmount[i] = records.Where(x => x.CategoryId == selectedCategories[i]).Sum(x => x.MoneyAmount);
            }

            return new KeyValuePair<int?[], decimal[]>(selectedCategories, moneyAmount);
        }
    }
}
