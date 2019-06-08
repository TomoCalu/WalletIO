using Microsoft.EntityFrameworkCore;
using WalletIO.Entities;

namespace WalletIO.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EntryType> EntryType { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}