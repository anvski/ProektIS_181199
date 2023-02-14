using Eshop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.ViewModels
{
    public class UsersLoad
    {
        public EshopAppUser user { get; set; }
        public string role { get; set; }
    }
}
