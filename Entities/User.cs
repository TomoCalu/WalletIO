using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WalletIO.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string CreatedTimestamp { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}