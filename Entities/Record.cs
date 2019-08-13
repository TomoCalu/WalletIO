using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletIO.Entities
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal MoneyAmount { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string CreatedTimestamp { get; set; }
        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }

    }
}
