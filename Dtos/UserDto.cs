using System;
using System.ComponentModel.DataAnnotations;

namespace WalletIO.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CurrentTimestamp { get; set; }
    }
}