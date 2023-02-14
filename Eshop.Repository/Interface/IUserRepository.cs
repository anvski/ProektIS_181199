using Eshop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Repository.Interface
{
    public interface IUserRepository
    { 
        IEnumerable<EshopAppUser> GetAll();
        EshopAppUser Get(string id);
        void Insert(EshopAppUser entity);
        void Update(EshopAppUser entity);
        void Delete(EshopAppUser entity);
        bool Exists(string id);

        public bool emailExists(string email);
        EshopAppUser GetWithOrders(string userId);
    }
}
