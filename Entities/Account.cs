using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletIO.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public decimal MoneyAmount { get; set; }
        public string CreatedTimestamp { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
