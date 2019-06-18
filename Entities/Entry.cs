using System.Collections.Generic;

namespace WalletIO.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal MoneyAmount { get; set; }
        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
        public string CreatedTimestamp { get; set; }
    }
}
