using Eshop.Domain.DomainModels;
using Eshop.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Services.Interface
{
    public interface IUserCartService
    {
        public UserCartLoader getUserCartInfo(string userId); 
        public bool placeOrder(string userId);

        public TicketInShoppingCart getTicketFromUserCart(Guid? id, string userId);

        public bool removeTicketFromCart(Guid id, string userId);
    }
}
