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
        int CountAccounts(int idUser);
        void DecreaseAccountMoneyAmount(int idAccount, decimal moneyAmount);
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
            return _context.Accounts.Where(x => x.UserId == idUser).ToList();
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

        public int CountAccounts(int idUser) {
            int numberOfAccounts = _context.Accounts.Where(x => x.UserId == idUser).ToList().Count();
            return numberOfAccounts;
        }

        public void DecreaseAccountMoneyAmount(int idAccount, decimal moneyAmount)
        {
            var selectedAccount = _context.Accounts.Find(idAccount);
            selectedAccount.MoneyAmount = selectedAccount.MoneyAmount - moneyAmount;

            _context.Accounts.Update(selectedAccount);
            _context.SaveChanges();
        }
    }
}
