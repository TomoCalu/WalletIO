using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalletIO.Entities;
using WalletIO.Helpers;

namespace WalletIO.Service
{
    public interface IAccountService
    {
        IEnumerable<Account> GetByIdUser(int idUser);
        void AddNew(Account account);
        void Update(Account account);
        void Delete(int idAccount);
    }

    public class AccountService : IAccountService
    {
        private DataContext _context;

        public AccountService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Account> GetByIdUser(int idUser)
        {
            return _context.Accounts.Where(x => x.User.Id == idUser).ToList();
        }

        public void AddNew(Account account)
        {
            account.CreatedTimestamp = DateTime.Now.ToString();

            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void Update(Account account)
        {
            var newAccount = _context.Accounts.Find(account.Id);

            if (newAccount == null)
                throw new AppException("Account not found");

            newAccount.MoneyAmount = account.MoneyAmount;
            newAccount.Name = account.Name;

            _context.Accounts.Update(newAccount);
            _context.SaveChanges();
        }
        public void Delete(int idAccount)
        {
            var account = _context.Accounts.Find(idAccount);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }
    }
}
