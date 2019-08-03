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
    public interface IRecordService
    {
        IEnumerable<Record> GetByIdAccount(int idAccount);
        void AddNew(Record record);
        void Update(Record record);
        void Delete(int idRecord);
    }

    public class RecordService : IRecordService
    {
        private DataContext _context;

        public RecordService(DataContext context)
        {
            _context = context;
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
    }
}
