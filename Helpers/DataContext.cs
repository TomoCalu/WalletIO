using System;
using Microsoft.EntityFrameworkCore;
using WalletIO.Entities;

namespace WalletIO.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EntryType> EntryTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Entry> Entries { get; set; }

    }
}