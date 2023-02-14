using Eshop.Domain.DomainModels;
using Eshop.Domain.ViewModels;
using Eshop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Eshop.Repository.Interface;
using System.Linq;

namespace Eshop.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<TicketInShoppingCart> _TISrepository;

        public TicketService(IRepository<Ticket> repository, IUserRepository userRepository, IRepository<TicketInShoppingCart> TISrepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _TISrepository = TISrepository;
        }
        public bool AddToUserCart(TicketToCart ticketToCart, string UserId)
        {
            var user = _userRepository.Get(UserId);
            var userShoppingCart = user.UserShoppingCart;

            var UserTicketsInShoppingCart = user.UserShoppingCart.TicketInShoppingCarts;

            bool flag = false;

            TicketInShoppingCart flaggedItem = null;

            if(ticketToCart == null)
            {
                return false;
            }
            if(UserId == null)
            {
                return false;
            }

            if(UserTicketsInShoppingCart.Count > 0)
            {
                foreach(var item in UserTicketsInShoppingCart)
                {
                    if(item.TicketId.Equals(ticketToCart.ticket.ID) && item.ShoppingCart.OwnerId.Equals(user.Id))
                    {
                        flag = true;
                        flaggedItem = item;
                    }
                }
            }

            if (flag)
            {
                var updateValue = flaggedItem.Quantity + ticketToCart.Quantity;
                flaggedItem.Quantity = updateValue;
                _TISrepository.Update(flaggedItem);
                return true;
            }
            else
            {
                TicketInShoppingCart ticketInShoppingCart = new TicketInShoppingCart
                {
                    TicketId = ticketToCart.ticket.ID,
                    Ticket = ticketToCart.ticket,
                    ShoppingCartId = userShoppingCart.ID,
                    ShoppingCart = userShoppingCart,
                    Quantity = ticketToCart.Quantity
                };
                _TISrepository.Insert(ticketInShoppingCart);
                return true;
            }
            
        }

        public void createNewTicket(Ticket t)
        {
            _repository.Insert(t);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = _repository.Get(id);
            _repository.Delete(ticket);

        }

        public List<Ticket> GetAllTickets()
        {
            return _repository.GetAll().ToList();
        }

        public Ticket GetTicket(Guid? id)
        {
            return _repository.Get(id);
        }

        public TicketToCart GetShoppingCartInfo(Guid? id)
        {
            var ticket = _repository.Get(id);
            TicketToCart ticketToCart = new TicketToCart();
            ticketToCart.ticket = ticket;
            return ticketToCart;
        }

        public void updateExistingTIcket(Ticket t)
        {
            _repository.Update(t);
        }

        public bool checkTicketExists(Guid id)
        {
            return _repository.Exists(id);
        }

        public List<Ticket> GetTicketsFromGenre(string genre)
        {
            if(genre == null)
            {
                return this.GetAllTickets();
            }
            return _repository.GetAll().Where(t => t.Genre.Equals(genre)).ToList();
        }
    }
}
