using Eshop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Domain.ViewModels
{
    public class TicketToCart
    {
        public Ticket ticket { get; set; }
        public int Quantity { get; set; }

    }
}
