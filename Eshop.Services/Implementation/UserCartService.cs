using Eshop.Domain.DomainModels;
using Eshop.Domain.ViewModels;
using Eshop.Repository.Interface;
using Eshop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.Services.Implementation
{
    public class UserCartService : IUserCartService
    {
        protected readonly IRepository<ShoppingCart> _repository;
        protected readonly IUserRepository _userRepository;
        protected readonly IRepository<TicketInShoppingCart> _TISrepository;
        protected readonly IRepository<Order> _orderRepository;
        protected readonly IRepository<TicketsInOrder> _OITrepository; 

        public UserCartService(IRepository<ShoppingCart> repository, 
            IUserRepository userRepository, IRepository<TicketInShoppingCart> TISrepository,
            IRepository<Order> orderRepository, IRepository<TicketsInOrder> OITrepository)
        {
            _repository = repository; 
            _userRepository = userRepository; 
            _TISrepository = TISrepository; 
            _orderRepository = orderRepository;
            _OITrepository = OITrepository;
        }
        public TicketInShoppingCart getTicketFromUserCart(Guid? id, string userId)
        {
            var loggedUser = _userRepository.Get(userId);
            var allTicketsInShoppingCart = loggedUser.UserShoppingCart.TicketInShoppingCarts.ToList();

            foreach(var ticketInShoppingCart in allTicketsInShoppingCart)
            {
                if (ticketInShoppingCart.TicketId.Equals(id))
                {
                    return ticketInShoppingCart;
                }
            }
            return null;
        }

        public UserCartLoader getUserCartInfo(string userId)
        {
            var loggedUser = _userRepository.Get(userId);

            //presmetuvanje totalna suma
            var AllTickets = loggedUser.UserShoppingCart.TicketInShoppingCarts.ToList();

            double sum = 0;

            foreach (var item in AllTickets)
            {
                sum += item.Quantity * item.Ticket.Price;
            }
            UserCartLoader loader = new UserCartLoader
            {
                ticketsInShoppingCart = loggedUser.UserShoppingCart.TicketInShoppingCarts.ToList(),

                totalPrice = sum
            };

            return loader;
        }

        public bool placeOrder(string userId)
        {
            var loggedUser = _userRepository.Get(userId);
            var AllTickets = loggedUser.UserShoppingCart.TicketInShoppingCarts.ToList();
            var shoppingCart = loggedUser.UserShoppingCart;

            if (AllTickets.Count == 0)
            {
                return false;
            }


            double sum = 0;

            foreach (var item in AllTickets)
            {
                sum += item.Quantity * item.Ticket.Price;
            }

            Order order = new Order
            {
                ID = Guid.NewGuid(),
                OwnerId = loggedUser.Id,
                price = sum,
                Owner = loggedUser,
            };

           // _orderRepository.Insert(order);

            List<TicketsInOrder> ticketsInOrders = new List<TicketsInOrder>();
            ticketsInOrders = shoppingCart.TicketInShoppingCarts
                .Select(t => new TicketsInOrder
                {
                    ID = Guid.NewGuid(),
                    OrderId = order.ID,
                    Order = order,
                    Ticket = t.Ticket,
                    TicketId = t.TicketId,
                }).ToList();

            foreach (TicketsInOrder item in ticketsInOrders)
            {
                _OITrepository.Insert(item);
            }

            shoppingCart.TicketInShoppingCarts.Clear();
            _repository.Update(shoppingCart);

            _userRepository.Update(loggedUser);

            return true;
        }

        public bool removeTicketFromCart(Guid id, string userId)
        {
            var loggedUser = _userRepository.Get(userId);

            var cart = loggedUser.UserShoppingCart;

            List<TicketInShoppingCart> AllTickets = cart.TicketInShoppingCarts.ToList();

            foreach(TicketInShoppingCart ticketInShoppingCart in AllTickets)
            {
                if (ticketInShoppingCart.TicketId.Equals(id))
                {
                    _TISrepository.Delete(ticketInShoppingCart);
                    return true;
                }
            }
            return false;
        }

    }
}
