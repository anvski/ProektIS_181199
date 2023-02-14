using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Domain.DomainModels
{
    public class TicketsInOrder : BaseEntity
    {
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
