using System.Collections.Generic;

namespace WalletIO.Entities
{
    public class EntryType
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual ICollection<Category> Category { get; set; }
    }
}
