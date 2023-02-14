using Eshop.Domain.Identity;
using Eshop.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<EshopAppUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<EshopAppUser>();
        }
        public IEnumerable<EshopAppUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public bool Exists(string id)
        {
            return entities.Any(s => s.Id == id);
        }
        public bool emailExists(string email)
        {
            return entities.Any(s => s.Email == email);
        }
        public EshopAppUser Get(string id)
        {
            return entities.Include(u => u.UserShoppingCart)
                .Include(u => u.UserShoppingCart.TicketInShoppingCarts)
                .Include("UserShoppingCart.TicketInShoppingCarts.Ticket")
                .Include(u => u.Orders)
                .SingleOrDefault(s => s.Id == id);
        }

        public void Insert(EshopAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(EshopAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(EshopAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public EshopAppUser GetWithOrders(string userId)
        {
            return entities.Include(u => u.Orders).ThenInclude(o => o.TicketsInOrder)
                .ThenInclude(t => t.Ticket)
            .SingleOrDefault(s => s.Id == userId);
                
        }

    }
}

