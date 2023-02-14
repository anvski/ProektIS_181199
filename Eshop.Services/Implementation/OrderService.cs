using Eshop.Domain.DomainModels;
using Eshop.Domain.Identity;
using Eshop.Repository.Interface;
using Eshop.Services.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Eshop.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IRepository<Order> orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public DocumentModel createInvoice(string path, string userId, Guid id)
        {
            var orderInfo = this.GetOrdersForUser(userId).Orders.Where(o => o.ID == id).FirstOrDefault();


            var template = DocumentModel.Load(path);
            template.Content.Replace("{{User}}", orderInfo.Owner.UserName.ToString());
            template.Content.Replace("{{OrderId}}", orderInfo.ID.ToString());
            template.Content.Replace("{{Price}}", "$"+orderInfo.price.ToString());

            StringBuilder sb = new StringBuilder();

            foreach(var item in orderInfo.TicketsInOrder)
            {
                sb.AppendLine(item.Ticket.ProductName + ", Genre: " 
                    + item.Ticket.Genre + ", Date: " + item.Ticket.dateValid);
            }
            template.Content.Replace("{{ListTickets}}", sb.ToString());
            template.Content.Replace("{{ListCount}}", orderInfo.TicketsInOrder.Count.ToString());

            return template;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAll().ToList();
        }

        public Order GetOrder(Guid? id)
        {
            return _orderRepository.Get(id);
        }

        public EshopAppUser GetOrdersForUser(string userId)
        {
            var loggedUser = _userRepository.GetWithOrders(userId);

            return loggedUser;
        }
    }
}
