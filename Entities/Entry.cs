using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletIO.Entities
{
    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal MoneyAmount { get; set; }
        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
        public string CreatedTimestamp { get; set; }
    }
}
