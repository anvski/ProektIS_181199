using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.ViewModels
{
    public class UsersForImport
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
        public string Role { get; set; }
    }
}
