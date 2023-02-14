using Eshop.Domain.DomainModels;
using Eshop.Domain.Identity;
using Eshop.Domain.ViewModels;
using Eshop.Repository.Interface;
using Eshop.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<EshopAppUser> _userManager;

        public UserService (IUserRepository userRepository, UserManager<EshopAppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public bool addUsers(List<UsersForImport> users)
        {
            if (users == null)
            {
                return false;
            }
            foreach(var user in users)
            {
                var toAdd = new EshopAppUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    UserShoppingCart = new ShoppingCart()
                };

                if (_userRepository.emailExists(toAdd.Email))
                {
                    continue; // ne se dodava
                }
                else
                {
                   var result = _userManager.CreateAsync(toAdd, user.Password).Result;

                    if(!result.Succeeded)
                    {
                        return false;
                    }

                    result = _userManager.AddToRoleAsync(toAdd, user.Role).Result;

                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public EshopAppUser getUser(string userId)
        {
            return _userRepository.Get(userId);
        }

        public List<UsersLoad> getAllUsers()
        {
            var users = _userRepository.GetAll().ToList();
            List<UsersLoad> result = new List<UsersLoad>();

            foreach (var item in users)
            {
                result.Add(new UsersLoad { user = item});
            }

            return result;
        }
    }
}
