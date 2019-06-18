using System.Collections.Generic;

namespace WalletIO.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual User User { get; set; }
        public byte[] BankApi { get; set; }
        public string CreatedTimestamp { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
