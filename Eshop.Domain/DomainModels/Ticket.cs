using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductImage { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public int Rating { get; set; }

        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime dateValid { get; set; }
        [Required]
        public string Genre { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }

        public virtual ICollection<TicketsInOrder> Orders { get; set; }
    }
}
