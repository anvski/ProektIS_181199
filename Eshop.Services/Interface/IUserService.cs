using Eshop.Domain.Identity;
using Eshop.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Services.Interface
{
    public interface IUserService
    {
        public EshopAppUser getUser(string userId);
        public bool addUsers(List<UsersForImport> users);
        public List<UsersLoad> getAllUsers();
        
    }
}
