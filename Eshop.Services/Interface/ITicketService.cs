using Eshop.Domain.DomainModels;
using Eshop.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetTicket(Guid? id);
        void createNewTicket(Ticket t);
        void updateExistingTIcket(Ticket t);
        TicketToCart GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddToUserCart(TicketToCart ticketToCart, string UserId);
        bool checkTicketExists(Guid id);

        List<Ticket> GetTicketsFromGenre(string genre);



    }
}
