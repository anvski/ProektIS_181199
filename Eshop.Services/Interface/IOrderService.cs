using Eshop.Domain.DomainModels;
using Eshop.Domain.Identity;
using GemBox.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Eshop.Services.Interface
{
    public interface IOrderService
    {
        public List<Order> GetAllOrders();

        public EshopAppUser GetOrdersForUser(string userId);

        public Order GetOrder(Guid? id);

        public DocumentModel createInvoice(string path, string userId, Guid id);

    }
}
