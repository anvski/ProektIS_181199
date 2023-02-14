using Eshop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Domain.ViewModels
{
    public class UserCartLoader
    {
        public ICollection<TicketInShoppingCart> ticketsInShoppingCart { get; set; }

        public double totalPrice { get; set; }
    }
}
