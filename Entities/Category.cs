﻿using System.Collections.Generic;

namespace WalletIO.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual EntryType EntryType { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}