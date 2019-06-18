using System.Collections.Generic;

namespace WalletIO.Entities
{
    public class EntryType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
