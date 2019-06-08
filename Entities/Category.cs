namespace WalletIO.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual EntryType EntryType { get; set; }
    }
}