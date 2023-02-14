using Eshop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public EshopAppUser Owner { get; set; }

        public virtual ICollection<TicketsInOrder> TicketsInOrder{ get; set; }

    }
}
